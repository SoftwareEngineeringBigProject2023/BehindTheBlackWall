using System;
using SEServer.Data;

namespace SEServer.Client
{
    public class ClientInstance
    {
        public int UserId { get; set; } = -1;
        public ClientWorld World { get; set; } = new ClientWorld();
        public ServerContainer ServerContainer { get; set; } = new ServerContainer();
        public bool IsConnected => ServerContainer.Get<IClientNetworkService>().IsConnected;
        
        public void Start()
        {
            World.ServerContainer = ServerContainer;
            
            ServerContainer.Init();
            ServerContainer.Start();
        }
        
        public void Update(float deltaTime)
        {
            // 处理服务器消息
            HandleReceiveServerMessage();
            
            // 世界主循环
            World.Update(deltaTime);
            
            // 发送消息给服务器
            HandleSendClientMessage();
        }

        private void HandleReceiveServerMessage()
        {
            var clientNetwork = ServerContainer.Get<IClientNetworkService>();
            
            while (clientNetwork.MessageQueue.TryDequeue(out var message))
            {
                try
                {
                    switch (message)
                    {
                        case AuthorizationReplyMessage replyMessage:
                            HandleAuthorizationReplyMessage(replyMessage);
                            break;
                        case IWorldMessage worldMessage:
                            HandleWorldMessage(worldMessage);
                            break;
                        default:
                            ServerContainer.Get<ILogger>().LogError($"未知的消息类型， Type = {message.GetType()}");
                            break;
                    }
                }
                catch (Exception e)
                {
                    ServerContainer.Get<ILogger>().LogError($"处理服务器消息出错");
                    ServerContainer.Get<ILogger>().LogError(e.ToString());
                }
            }
        }

        private void HandleAuthorizationReplyMessage(AuthorizationReplyMessage replyMessage)
        {
            UserId = replyMessage.UserId;
        }

        private void HandleWorldMessage(IWorldMessage worldMessage)
        {
            World.ReceiveMessageQueue.Enqueue(worldMessage);
        }

        private void HandleSendClientMessage()
        {
            var network = ServerContainer.Get<IClientNetworkService>();
            while (World.SendMessageQueue.TryDequeue(out var message))
            {
                switch (message)
                {
                    case SubmitEntityMessage replyMessage:
                        network.SendMessage(replyMessage);
                        break;
                }
            }
        }
    }
}