using System;
using MessagePack;
using MessagePack.Resolvers;
using SEServer.Data;
using UnityEngine;
using ILogger = SEServer.Data.ILogger;

namespace SEServer.Client
{
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
            try
            {
                return MessagePackSerializer.Serialize(data);
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
            var obj = MessagePackSerializer.Deserialize<T>(readBytes, MessagePackSerializer.DefaultOptions);
            return obj;
        }
        
        // 序列化注册部分
        private static bool serializerRegistered = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (!serializerRegistered)
            {
                StaticCompositeResolver.Instance.Register(
                    GeneratedResolver.Instance,
                    StandardResolver.Instance
                );

                var option = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);

                MessagePackSerializer.DefaultOptions = option;
                serializerRegistered = true;
            }
        }

#if UNITY_EDITOR


        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInitialize()
        {
            Initialize();
        }

#endif
    }
}