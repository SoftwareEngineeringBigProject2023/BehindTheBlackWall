using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Controller;
using Game.Framework;
using Game.GameSystem;
using SEServer.Client;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.Data.Message;
using SEServer.GameData;
using SEServer.GameData.Component;
using Sirenix.OdinInspector;
using UnityEngine;
using ILogger = SEServer.Data.Interface.ILogger;

namespace Game
{
    public class ClientBehaviour : SceneSingleton<ClientBehaviour>
    {
        public ClientInstance ClientInstance { get; set; }
        public EntityMapperManager EntityMapper { get; set; }
        
        /// <summary>
        /// 客户端网络实例帧率
        /// </summary>
        public int MAX_FRAME_RATE { get; set; } = 30;
        private float _deltaTime;
        
        public void Init()
        {
            var systemProvider = new SystemProvider();
            systemProvider.AddSystem(new HandleMoveInputSystem());
            systemProvider.AddSystem(new HandleShootInputSystem());
            
            ClientInstance = new ClientInstance();
            ClientInstance.ServerContainer.Add<ILogger>( new SimpleLogger());
            ClientInstance.ServerContainer.Add<IDataSerializer>( new SimpleSerializer());
            ClientInstance.ServerContainer.Add<IClientNetworkService>( new ClientNetworkService());
            ClientInstance.ServerContainer.Add<IComponentSerializer>( new ComponentSerializer());
            ClientInstance.ServerContainer.Add<ISystemProvider>(systemProvider);
            ClientInstance.Start();

            EntityMapper = new EntityMapperManager();
            EntityMapper.Init(ClientInstance.World);
            
            EntityMapper.RegisterControllerBuilder(new TransformControllerBuilder());
            EntityMapper.RegisterControllerBuilder(new GraphControllerBuilder());
            EntityMapper.RegisterControllerBuilder(new UnitGraphControllerBuilder());
            EntityMapper.AddSingletonController(new PlayerSingletonController());
        }

        public async UniTask Connect()
        {
            var networkService = ClientInstance.ServerContainer.Get<IClientNetworkService>() as ClientNetworkService;
            Debug.Assert(networkService != null, nameof(networkService) + " != null");
            if(networkService.IsConnected)
                return;
            
            await networkService.StartConnection();
            Debug.Log("连接成功");
        }

        private void Update()
        {
            if (ClientInstance.IsConnected)
            {
                UpdateClient();
            }
        }

        /// <summary>
        /// 客户端帧更新
        /// </summary>
        private void UpdateClient()
        {
            _deltaTime += Time.deltaTime;
            if (_deltaTime > 1.0f / MAX_FRAME_RATE)
            {
                // 更新客户端
                ClientInstance.Update(_deltaTime);
                // 更新实体映射
                EntityMapper.UpdateEntities();
                _deltaTime = 0;
            }
            
            EntityMapper.UpdateControllers();
        }

        [Button]
        public void TestAddPlayer()
        {
            var clientPlayer = ClientInstance.World.EntityManager.GetSingleton<PlayerSubmitGlobalComponent>();
            var message = new SubmitData()
            {
                Type = PlayerSubmitGlobalMessageType.CREATE_PLAYER,
                Arg0 = ClientInstance.World.PlayerId.Id
            };

            clientPlayer.AddSubmitMessage(message);
        }
    }
}