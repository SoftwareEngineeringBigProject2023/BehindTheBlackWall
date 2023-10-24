using System;

namespace SEServer.Data;

public static class MessageHelper
{
    public static MessageHeader PeekHeader(byte[] bytes, int offset, int size)
    {
        if (offset + MessageHeader.MESSAGE_HEADER_SIZE > size)
        {
            throw new ArgumentException("size is too small");
        }
        
        var header = new MessageHeader
        {
            DateLength = BitConverter.ToInt32(bytes, offset + 0),
            DataType = BitConverter.ToInt32(bytes, offset + 4),
            IsEnd = BitConverter.ToBoolean(bytes, offset + 8)
        };
        
        return header;
    }

    public static byte[] SerializeHeader(MessageHeader header)
    {
        var bytes = new byte[MessageHeader.MESSAGE_HEADER_SIZE];
        Array.Copy(BitConverter.GetBytes(header.DateLength), 0, bytes, 0, 4);
        Array.Copy(BitConverter.GetBytes(header.DataType), 0, bytes, 4, 4);
        Array.Copy(BitConverter.GetBytes(header.IsEnd), 0, bytes, 8, 1);
        return bytes;
    }
}