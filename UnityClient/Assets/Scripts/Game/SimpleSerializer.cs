using System;
using MessagePack;
using MessagePack.Resolvers;
using SEServer.Data;
using SEServer.Data.Interface;
using ILogger = SEServer.Data.Interface.ILogger;

namespace Game
{
    public class SimpleSerializer : IDataSerializer
    {
        public ServerContainer ServerContainer { get; set; }
        private MessagePackSerializerOptions Option { get; set; }
        public void Init()
        {
            StaticCompositeResolver.Instance.Register(
                MessagePack.SEServer.Data.Resolvers.GeneratedDataResolver.Instance,
                MessagePack.SEServer.GameData.Resolvers.GeneratedGameDataResolver.Instance,
                StandardResolver.Instance
            );

            
            Option = MessagePackSerializerOptions.Standard
                .WithResolver(StaticCompositeResolver.Instance)
                .WithCompression(MessagePackCompression.Lz4BlockArray);
        }

        public void Start()
        {
        
        }

        public void Stop()
        {
        
        }

        public byte[] Serialize<T>(T data)
        {
            try
            {
                return MessagePackSerializer.Serialize(data, Option);
            }
            catch (Exception e)
            {
                ServerContainer.Get<ILogger>().LogError(e.Message);
                ServerContainer.Get<ILogger>().LogError(e.StackTrace);
                throw;
            }
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
            var obj = MessagePackSerializer.Deserialize<T>(readBytes, Option);
            return obj;
        }
    }
}