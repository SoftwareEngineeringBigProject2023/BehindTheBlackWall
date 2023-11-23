using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePack;
using SEServer.Data;
using NativeWebSocket;
using SEServer.Data.Interface;
using SEServer.Data.Message;
using UnityEngine;
using ILogger = SEServer.Data.Interface.ILogger;
using Random = UnityEngine.Random;

namespace SEServer.Client
{
    public class ClientNetworkService : IClientNetworkService
    {
        public ServerContainer ServerContainer { get; set; }
        public Queue<IMessage> MessageQueue { get; } = new Queue<IMessage>();
        public bool IsConnected => WebSocket is { State: WebSocketState.Open };
        public WebSocket WebSocket { get; set; }
        private byte[] ReceiveBuffer { get; set; } = new byte[MessageHeader.BUFFER_SIZE];
        private int ReceiveBufferSize { get; set; } = 0;
        /// <summary>
        /// 未合并的消息分片
        /// </summary>
        private List<byte[]> UncombinedMessage { get; } = new List<byte[]>();

        public void Init()
        {
            
        }

        public void Start()
        {
            
        }
        
        public void Stop()
        {
            
        }
        
        public async UniTask StartConnection()
        {
            const int port = 33700;
            //WebSocket = new WebSocket($"ws://localhost:{port}/Game/");
            WebSocket = new WebSocket($"ws://175.178.23.165:{port}/Game/");

            WebSocket.OnOpen += OnOpen;

            WebSocket.OnError += OnError;

            WebSocket.OnClose += OnClose;

            WebSocket.OnMessage += OnMessage;
            
            UniTask.FromResult(WebSocket.Connect()).Forget();
            
            await UniTask.WaitUntil(() => WebSocket.State is WebSocketState.Open or WebSocketState.Closed);
        }


#if !UNITY_WEBGL || UNITY_EDITOR
        private async void DispatchMessageQueue()
        {
            while (true)
            {
                if (WebSocket.State == WebSocketState.Closed)
                {
                    break;
                }
                WebSocket.DispatchMessageQueue();
            
                await UniTask.DelayFrame(1);
            }
        }
#endif
        
        private void OnOpen()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            DispatchMessageQueue();
#endif
            ServerContainer.Get<ILogger>().LogInfo("Connection open!");
            var authMessage = new AuthorizationMessage()
            {
                UserId = Random.Range(0, 1000000),
            };
            SendMessage(authMessage);
        }

        private void OnError(string errormsg)
        {
            ServerContainer.Get<ILogger>().LogError($"Error: {errormsg}");
        }

        private void OnClose(WebSocketCloseCode closecode)
        {
            ServerContainer.Get<ILogger>().LogInfo($"Connection closed with code {closecode}");
        }

        private void OnMessage(byte[] data)
        {
            //ServerContainer.Get<ILogger>().LogInfo($"Message received: {data.Length} size");
            
            var receiveSize = data.Length;
            if (receiveSize + ReceiveBufferSize > MessageHeader.BUFFER_SIZE)
            {
                ServerContainer.Get<ILogger>().LogError("Receive buffer overflow!");
                ClearBuffer();
                return;
            }
            
            Array.Copy(data, 0, ReceiveBuffer, ReceiveBufferSize, receiveSize);
            ReceiveBufferSize += receiveSize;
            
            if (ReceiveBufferSize < MessageHeader.MESSAGE_HEADER_SIZE)
            {
                // 消息头未接收完全
                return;
            }
            
            var header = MessageHelper.PeekHeader(ReceiveBuffer, 0, ReceiveBufferSize);
            
            var headAndBodySize = MessageHeader.MESSAGE_HEADER_SIZE + header.DateLength;
            if (headAndBodySize > ReceiveBufferSize)
            {
                // 消息未完全接收
                return;
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
        
        private void ClearBuffer()
        {
            ReceiveBufferSize = 0;
        }
        
        public void Send(byte[] bytes)
        {
            WebSocket.Send(bytes);
        }
    
        public void SendMessage<T>(T message) where T : IMessage
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
    }
}