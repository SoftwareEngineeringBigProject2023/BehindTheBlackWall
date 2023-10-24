using System.Collections.Generic;
using SEServer.Data;

namespace SEServer.Client
{
    public interface IClientNetworkService : IService
    {
        public Queue<IMessage> MessageQueue { get; }
        bool IsConnected { get; }
        void SendMessage(IMessage message);
    }
}