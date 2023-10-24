using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using Cysharp.Threading.Tasks;
using SEServer.Data;

namespace SEServer.Core;

/// <summary>
/// 客户端状态
/// </summary>
public class ClientConnect
{
    public ClientConnect(HttpListenerContext context, ServerContainer serverContainer)
    {
        Context = context;
        ServerContainer = serverContainer;
    }

    public ServerContainer ServerContainer { get; set; }
    public ClientConnectState State { get; private set; } = ClientConnectState.Disconnected;
    public ConcurrentQueue<IMessage> MessageQueue { get; } = new ConcurrentQueue<IMessage>();
    private HttpListenerContext Context { get; set; }
    private WebSocket WebSocket { get; set; }
    private byte[] ReceiveBuffer { get; set; } = new byte[MessageHeader.BUFFER_SIZE];
    private int ReceiveBufferSize { get; set; } = 0;
    /// <summary>
    /// 未合并的消息分片
    /// </summary>
    private List<byte[]> UncombinedMessage { get; } = new List<byte[]>();
    /// <summary>
    /// 双向绑定的用户
    /// </summary>
    public User User { get; set; }

    public async UniTask ProcessWebSocketRequest()
    {
        HttpListenerWebSocketContext webSocketContext = await Context.AcceptWebSocketAsync(subProtocol: null);
        
        WebSocket = webSocketContext.WebSocket;
        var tmpBuffer = new byte[MessageHeader.BUFFER_SIZE];
        try
        {
            WebSocketReceiveResult result;
            while (true)
            {
                result = await WebSocket.ReceiveAsync(new ArraySegment<byte>(tmpBuffer), CancellationToken.None);
                
                if(result.CloseStatus.HasValue)
                {
                    break;
                }
                
                var receiveSize = result.Count;
                if (receiveSize + ReceiveBufferSize > MessageHeader.BUFFER_SIZE)
                {
                    ServerContainer.Get<ILogger>().LogError("Receive buffer overflow!");
                    ClearBuffer();
                    continue;
                }
                
                Array.Copy(tmpBuffer, 0, ReceiveBuffer, ReceiveBufferSize, receiveSize);
                ReceiveBufferSize += receiveSize;
                
                if (ReceiveBufferSize < MessageHeader.MESSAGE_HEADER_SIZE)
                {
                    // 消息头未接收完全
                    continue;
                }
                
                var header = MessageHelper.PeekHeader(ReceiveBuffer, 0, ReceiveBufferSize);
                
                var headAndBodySize = MessageHeader.MESSAGE_HEADER_SIZE + header.DateLength;
                if (headAndBodySize > ReceiveBufferSize)
                {
                    // 消息未完全接收
                    continue;
                }
                
                // 消息接收完全，开始处理
                if (header.IsEnd)
                {
                    byte[] messageBytes;
                    int totalSize;
                    if (UncombinedMessage.Count > 0)
                    {
                        totalSize = UncombinedMessage.Sum(m => m.Length) + header.DateLength;
                        // 合并分片
                        messageBytes = new byte[totalSize];
                        var offset = 0;
                        foreach (var uncombined in UncombinedMessage)
                        {
                            Array.Copy(uncombined, 0, messageBytes, offset, uncombined.Length);
                            offset += uncombined.Length;
                        }
                        Array.Copy(ReceiveBuffer, MessageHeader.MESSAGE_HEADER_SIZE, messageBytes, offset, headAndBodySize);
                        
                        UncombinedMessage.Clear();
                    }
                    else
                    {
                        totalSize = header.DateLength;
                        messageBytes = new byte[header.DateLength];
                        Array.Copy(ReceiveBuffer, MessageHeader.MESSAGE_HEADER_SIZE, messageBytes, 
                            0, header.DateLength);
                    }

                    var receiveMessage = header.DeserializeMessage(ServerContainer.Get<IDataSerializer>(), messageBytes, 0, totalSize);
                    MessageQueue.Enqueue(receiveMessage);
                }
                else
                {
                    // 消息分片，等待下一次接收
                    var message = new byte[header.DateLength];
                    Array.Copy(ReceiveBuffer, MessageHeader.MESSAGE_HEADER_SIZE, message, 0, header.DateLength);
                    UncombinedMessage.Add(message);
                }
                
                // 前移缓冲区
                var remainSize = ReceiveBufferSize - headAndBodySize;
                if (remainSize > 0)
                {
                    Array.Copy(ReceiveBuffer, headAndBodySize, ReceiveBuffer, 0, remainSize);
                }
                ReceiveBufferSize = remainSize;
            }

            await WebSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
        catch (Exception ex)
        {
            ServerContainer.Get<ILogger>().LogError(ex);
        }
        
        State = ClientConnectState.Disconnected;
    }

    private void ClearBuffer()
    {
        ReceiveBufferSize = 0;
    }

    public void Start()
    {
        State = ClientConnectState.Connected;
        UniTask.Run(ProcessWebSocketRequest);
    }
    
    public void Disconnect()
    {
        WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server stop", CancellationToken.None);
        State = ClientConnectState.Disconnected;
    }

    public void Send(byte[] bytes)
    {
        WebSocket.SendAsync(bytes, WebSocketMessageType.Binary, true, CancellationToken.None);
    }
    
    public void SendMessage(IMessage message)
    {
        var bytes = ServerContainer.Get<IDataSerializer>().Serialize(message);
        if(bytes.Length > MessageHeader.MAX_MESSAGE_SIZE)
        {
            // 消息过大，分片发送
            for (int i = 0; i < bytes.Length; i += MessageHeader.MAX_MESSAGE_SIZE)
            {
                var size = Math.Min(MessageHeader.MAX_MESSAGE_SIZE, bytes.Length - i);
                var header = new MessageHeader
                {
                    DateLength = size,
                    DataType = MessageHeader.GetMessageTypeCode(message),
                    IsEnd = i + size >= bytes.Length
                };
                var headerBytes = MessageHelper.SerializeHeader(header);
                var sendBytes = new byte[headerBytes.Length + size];
                Array.Copy(headerBytes, 0, sendBytes, 0, headerBytes.Length);
                Array.Copy(bytes, i, sendBytes, headerBytes.Length, size);
                Send(sendBytes);
            }
        }
        else
        {
            // 消息不大，直接发送
            var header = new MessageHeader
            {
                DateLength = bytes.Length,
                DataType = MessageHeader.GetMessageTypeCode(message),
                IsEnd = true
            };
            var headerBytes = MessageHelper.SerializeHeader(header);
            var sendBytes = new byte[headerBytes.Length + bytes.Length];
            Array.Copy(headerBytes, 0, sendBytes, 0, headerBytes.Length);
            Array.Copy(bytes, 0, sendBytes, headerBytes.Length, bytes.Length);
            Send(sendBytes);
        }
    }

    public void SetAuthorized()
    {
        if(State != ClientConnectState.Disconnected)
        {
            State = ClientConnectState.Authorized;
        }
    }
}