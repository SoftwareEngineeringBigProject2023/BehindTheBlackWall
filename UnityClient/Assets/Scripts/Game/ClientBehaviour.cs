using System.Collections.Generic;
using Game.GameSystem;
using SEServer.Client;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.Data.Message;
using SEServer.GameData;
using Sirenix.OdinInspector;
using UnityEngine;
using ILogger = SEServer.Data.Interface.ILogger;

namespace Game
{
    public class ClientBehaviour : MonoBehaviour
    {
        public static ClientBehaviour Instance { get; private set; }
        public ClientInstance ClientInstance { get; set; }
        public EntityMapperManager EntityMapper { get; set; }
        
        /// <summary>
        /// 客户端网络实例帧率
        /// </summary>
        public int MAX_FRAME_RATE { get; set; } = 30;
        private float _deltaTime;
        
        public void Start()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;

            var systemProvider = new SystemProvider();
            systemProvider.AddSystem(new InputSystem());
            
            ClientInstance = new ClientInstance();
            ClientInstance.ServerContainer.Add<ILogger>( new SimpleLogger());
            ClientInstance.ServerContainer.Add<IDataSerializer>( new SimpleSerializer());
            ClientInstance.ServerContainer.Add<IClientNetworkService>( new ClientNetworkService());
            ClientInstance.ServerContainer.Add<IComponentSerializer>( new ComponentSerializer());
            ClientInstance.ServerContainer.Add<ISystemProvider>(systemProvider);
            ClientInstance.Start();
            
            EntityMapper = new EntityMapperManager();
            EntityMapper.Init(ClientInstance.World);
        }

        private void Update()
        {
            UpdateClient();
        }

        /// <summary>
        /// 客户端帧更新
        /// </summary>
        private void UpdateClient()
        {
            _deltaTime += Time.deltaTime;
            if (_deltaTime > 1.0f / MAX_FRAME_RATE && ClientInstance.IsConnected)
            {
                ClientInstance.Update(_deltaTime);
                EntityMapper.UpdateEntities();
                _deltaTime = 0;
            }
        }

        [Button]
        public void TestAddPlayer()
        {
            var clientPlayer = ClientInstance.World.EntityManager.GetSingleton<PlayerMessageComponent>();
            var message = new SubmitData()
            {
                Type = PlayerSubmitMessageType.CREATE_PLAYER,
                Arg0 = ClientInstance.World.PlayerId.Id
            };

            clientPlayer.AddSubmitMessage(message);
        }
    }
}