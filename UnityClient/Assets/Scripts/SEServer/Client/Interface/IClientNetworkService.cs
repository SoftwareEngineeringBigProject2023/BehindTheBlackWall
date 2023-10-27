using System.Collections.Generic;
using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.Client
{
    public interface IClientNetworkService : IService
    {
        public Queue<IMessage> MessageQueue { get; }
        bool IsConnected { get; }
        void SendMessage<T>(T message) where T : IMessage;
    }
}