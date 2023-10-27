using MessagePack;
using SEServer.Data;

namespace SEServer.Cil;

public class SimpleSerializer : IDataSerializer
{
    public ServerContainer ServerContainer { get; set; }
    public void Init()
    {
        
    }

    public void Start()
    {
        
    }

    public void Stop()
    {
        
    }

    public byte[] Serialize<T>(T data)
    {
        return MessagePackSerializer.Serialize(data.GetType() ,data);
    }

    public int Serialize<T>(T data, byte[] bytes, int offset)
    {
        var serializedData = Serialize(data);
        Array.Copy(serializedData, 0, bytes, offset, serializedData.Length);
        return serializedData.Length;
    }

    public T Deserialize<T>(byte[] bytes, int offset, int size)
    {
        var readBytes = new ReadOnlyMemory<byte>(bytes, offset, size);
        var obj = MessagePackSerializer.Deserialize<T>(readBytes, MessagePackSerializerOptions.Standard);
        return obj;
    }
}