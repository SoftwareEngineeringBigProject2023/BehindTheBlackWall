using System;
using MessagePack;

namespace SEServer.Data;

public class MessageHeader
{
    /// <summary>
    /// 缓冲区大小 2MB
    /// </summary>
    public const int BUFFER_SIZE = 2 * 1024 * 1024;
    /// <summary>
    /// 单个消息最大大小 1MB
    /// </summary>
    public const int MAX_MESSAGE_SIZE = 1024 * 1024;
    
    public const int MESSAGE_HEADER_SIZE = 9;
    
    public const int DATA_TYPE_AUTHORIZATION = 1;
    public const int DATA_TYPE_SNAPSHOT = 2;
    public const int DATA_TYPE_SYNC = 3;
    public const int DATA_TYPE_SUBMIT = 4;
    
    public int DateLength { get; set; }
    public int DataType { get; set; }
    public bool IsEnd { get; set; }

    public static int GetMessageTypeCode(IMessage message)
    {
        switch (message)
        {
            case AuthorizationMessage:
                return DATA_TYPE_AUTHORIZATION;
            case SnapshotMessage:
                return DATA_TYPE_SNAPSHOT;
            case SyncEntityMessage:
                return DATA_TYPE_SYNC;
            case SubmitEntityMessage:
                return DATA_TYPE_SUBMIT;
        }
        
        throw new ArgumentException("Unknown message type : " + message.GetType().Name);
    }
    
    public IMessage DeserializeMessage(IDataSerializer serializer, byte[] bytes, int offset, int totalLength)
    {
        switch (DataType)
        {
            case DATA_TYPE_AUTHORIZATION:
                return serializer.Deserialize<AuthorizationMessage>(bytes, offset, totalLength);
            case DATA_TYPE_SNAPSHOT:
                return serializer.Deserialize<SnapshotMessage>(bytes, offset, totalLength);
            case DATA_TYPE_SYNC:
                return serializer.Deserialize<SyncEntityMessage>(bytes, offset, totalLength);
            case DATA_TYPE_SUBMIT:
                return serializer.Deserialize<SubmitEntityMessage>(bytes, offset, totalLength);
        }
        
        throw new ArgumentException("Unknown message type : " + DataType);
    }
}