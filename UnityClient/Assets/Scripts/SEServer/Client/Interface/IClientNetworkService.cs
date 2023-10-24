using System.Collections.Generic;
using SEServer.Data;

namespace SEServer.Client
{
    public interface IClientNetworkService : IService
    {
        public Queue<IMessage> MessageQueue { get; }
        bool IsConnected { get; }
        void SendMessage<T>(T message) where T : IMessage;
    }
}