using System.Diagnostics;
using SEServer.Data;

namespace SEServer.Core;

public class ServerInstance
{
    private bool _endFlog = false;
    public ServerContainer ServerContainer { get; set; } = new ServerContainer();
    public UserManager UserManager { get; } = new UserManager();
    public List<ServerWorld> Worlds { get; } = new List<ServerWorld>();
    public Time Time { get; } = new Time();
    
    public void StartGame()
    {
        Time.Init();
        
        ServerContainer.Init();
        ServerContainer.Start();
        
        // TODO: 测试性添加一个世界，后续修改
        var serverWorld = new ServerWorld()
        {
            ServerContainer = ServerContainer
        };
        serverWorld.EntityManager.CreateEntity();
        Worlds.Add(serverWorld);

        MainLoop();
    }

    public void EndGame()
    {
        _endFlog = true;
    }

    private void MainLoop()
    {
        while (true)
        {
            Time.StartFrame();
            // 处理客户端消息
            HandleReceiveClientMessages();
            
            // 世界主循环
            foreach (var world in Worlds)
            {
                world.Update(Time.DeltaTime);
            }
            
            // 用户状态处理
            HandleUserState();

            // 发送客户端消息
            HandleSendClientMessages();
            
            if (_endFlog)
            {
                EndGame();
                return;
            }

            if (Time.CurFrame % (Time.MaxFps * 5) == 0)
            {
                ServerContainer.Get<ILogger>().LogInfo($"服务器信息： Fps: {Time.Fps:D2} \t负载：{Time.LoadPercentage * 100:F}%");
            }
            
            // 自旋
            Time.EndFrame();
        }
    }

    private void HandleReceiveClientMessages()
    {
        var serverNetworkService = ServerContainer.Get<IServerNetworkService>();
        var clientConnects = serverNetworkService.ClientConnects;
        foreach (var clientConnect in clientConnects)
        {
            HandleReceiveClientMessage(clientConnect);
        }
        
        serverNetworkService.RemoveUnconnectedClient();
    }

    private void HandleReceiveClientMessage(ClientConnect clientConnect)
    {
        if(clientConnect.State == ClientConnectState.Disconnected) 
            return;

        while (clientConnect.MessageQueue.TryDequeue(out var message))
        {
            try
            {
                if(message is AuthorizationMessage authorizationMessage)
                {
                    HandleAuthorizationMessage(clientConnect, authorizationMessage);
                    continue;
                }

                if (clientConnect.State != ClientConnectState.Authorized)
                {
                    // 未授权的客户端不接受其他消息
                    continue;
                }

                switch (message)
                {
                    case IWorldMessage worldMessage:
                        HandleWorldMessage(clientConnect.User, worldMessage);
                        break;
                    default:
                        ServerContainer.Get<ILogger>().LogError($"未知的消息类型，Id = {clientConnect.User.Id} Type = {message.GetType()}");
                        break;
                }
            }
            catch (Exception e)
            {
                ServerContainer.Get<ILogger>().LogError($"处理用户消息出错，Id = {clientConnect.User.Id}");
                ServerContainer.Get<ILogger>().LogError(e.ToString());
            }
        }
    }

    /// <summary>
    /// 处理用户认证
    /// </summary>
    /// <param name="clientConnect"></param>
    /// <param name="authorizationMessage"></param>
    private void HandleAuthorizationMessage(ClientConnect clientConnect, AuthorizationMessage authorizationMessage)
    {
        var userId = new UserId()
        {
            Id = authorizationMessage.UserId
        };
        
        // 先检查是否重复登录
        var user = UserManager.CreateOrGetUser(userId);
        if (user.ClientConnect != null)
        {
            // 重复登录，将之前的连接断开
            var oldClientConnect = user.ClientConnect;
            oldClientConnect.Disconnect();
            user.ClientConnect = clientConnect;
        }
        else
        {
            // 新登录
            user.ClientConnect = clientConnect;
        }
        user.NewClientConnect = true;

        ServerContainer.Get<ILogger>().LogInfo($"用户认证，Id = {user.Id}");
        
        clientConnect.User = user;
        clientConnect.SetAuthorized();
    }
    
    /// <summary>
    /// 处理用户状态
    /// </summary>
    private void HandleUserState()
    {
        for (var index = 0; index < UserManager.Users.Count; index++)
        {
            var user = UserManager.Users[index];
            
            ServerWorld? bindWorld = null;
            if (user is { NewClientConnect: true, ClientConnect: {State: ClientConnectState.Authorized} })
            {
                // 新连接
                user.NewClientConnect = false;

                bindWorld = null;
                if (user.BindWorld != WId.Invalid)
                {
                    bindWorld = Worlds.FirstOrDefault(world => world.Id == user.BindWorld);
                    
                }

                if (bindWorld == null)
                {
                    // TODO: 临时绑定第一个世界，后续再改进为动态绑定
                    bindWorld = Worlds[0];
                    // 绑定世界
                    user.BindWorld = bindWorld.Id;
                }
                
                // 发送快照信息
                var sendSnapshotMessage = new SnapshotMessage()
                {
                    Snapshot = bindWorld.NearestSnapshot
                };
                user.ClientConnect.SendMessage(sendSnapshotMessage);
                
                // 发送补偿信息
                foreach (var worldMessage in bindWorld.IncrementalStateInfo)
                {
                    switch (worldMessage)
                    {
                        case SubmitEntityMessage submitMessage:
                            user.ClientConnect.SendMessage(submitMessage);
                            break;
                        default:
                            ServerContainer.Get<ILogger>().LogError($"不支持的消息类型，Id = {user.Id} Type = {worldMessage.GetType()}");
                            break;
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// 接收用户提交的实体信息
    /// </summary>
    /// <param name="user"></param>
    /// <param name="worldMessage"></param>
    private void HandleWorldMessage(User user, IWorldMessage worldMessage)
    {
        if (user.BindWorld == WId.Invalid)
        {
            return;
        }
        
        var world = Worlds.First(world => world.Id == user.BindWorld);
        world.ReceiveMessageQueue.Enqueue(worldMessage);
    }
    
    private void HandleSendClientMessages()
    {
        foreach (var world in Worlds)
        {
            var allUser = UserManager.Users.Where(user => user.BindWorld == world.Id).ToArray();
            while (world.SendMessageQueue.TryDequeue(out var message))
            {
                for (var index = 0; index < allUser.Length; index++)
                {
                    var user = allUser[index];
                    TrySendWorldMessage(user, message);
                }
            }
        }
    }

    private void TrySendWorldMessage<T>(User user, T message) where T : IWorldMessage
    {
        if(user.ClientConnect == null) 
            return;
        
        if(user.ClientConnect.State != ClientConnectState.Authorized)
            return;
            
        user.ClientConnect.SendMessage(message);
    }
}