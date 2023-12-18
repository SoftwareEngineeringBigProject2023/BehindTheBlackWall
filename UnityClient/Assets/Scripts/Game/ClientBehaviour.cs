using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Game.Component;
using Game.Controller;
using Game.Framework;
using Game.GameSystem;
using SEServer.Client;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.Data.Message;
using SEServer.GameData;
using SEServer.GameData.Component;
using SEServer.GameData.CTData;
using SEServer.GameData.Data;
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
            systemProvider.AddSystem(new PlayerMessageHandleSystem());
            systemProvider.AddSystem(new HandleMoveInputSystem());
            systemProvider.AddSystem(new HandleShootInputSystem());

            ClientInstance = new ClientInstance();
            ClientInstance.ServerContainer.Add<ILogger>( new SimpleLogger());
            ClientInstance.ServerContainer.Add<IDataSerializer>( new SimpleSerializer());
            ClientInstance.ServerContainer.Add<IClientNetworkService>(new ClientNetworkService());
            ClientInstance.ServerContainer.Add<IComponentSerializer>( new ComponentSerializer());
            ClientInstance.ServerContainer.Add<ISystemProvider>(systemProvider);
            
            // 配置表
            var configTable = new ConfigTable(OnLoadData, OnLoadAllData);
            configTable.ConfigTableNames.Add(new ConfigTableName(ConStr.MapDataRoot, typeof(GameMapCTData), true));
            configTable.ConfigTableNames.Add(new ConfigTableName(ConStr.BulletCTDataRoot, typeof(BulletCTData)));
            configTable.ConfigTableNames.Add(new ConfigTableName(ConStr.WeaponCTDataRoot, typeof(WeaponCTData)));
            ClientInstance.ServerContainer.Add<IConfigTable>(configTable);
            
            ClientInstance.Start();

            EntityMapper = new EntityMapperManager();
            EntityMapper.Init(ClientInstance.World);
            
            EntityMapper.RegisterControllerBuilder(new TransformControllerBuilder());
            EntityMapper.RegisterControllerBuilder(new GraphControllerBuilder());
            EntityMapper.RegisterControllerBuilder(new UnitGraphControllerBuilder());
            EntityMapper.RegisterControllerBuilder(new MapControllerBuilder());
            EntityMapper.RegisterControllerBuilder(new HpViewControllerBuilder());
            EntityMapper.AddSingletonController(new PlayerSingletonController());
            EntityMapper.AddSingletonController(new ScoreSingletonController());
            EntityMapper.AddSingletonController(new GameEffectSingletonController());
        }

        private string[] OnLoadAllData(string path)
        {
            var jsons = new List<string>();
            foreach (var textAsset in GameManager.Res.LoadAllAssets<TextAsset>(path, ".json", true))
            {
                jsons.Add(textAsset.text);
            }
            
            return jsons.ToArray();
        }

        private string OnLoadData(string path)
        {
            return GameManager.Res.LoadAsset<TextAsset>(path).text;
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
        
        public EId GetPlayerEId()
        {
            return ClientInstance.World.EntityManager.GetSingleton<LocalPlayerInfoSingletonComponent>().PlayerEntityId;
        }
        
        private void Update()
        {
            if (ClientInstance == null)
                return;
            
            if (ClientInstance.IsConnected)
            {
                UpdateClient();
            }
        }

        private void FixedUpdate()
        {

        }

        private void OnApplicationQuit()
        {
            if (ClientInstance.IsConnected)
            {
                ClientInstance.Stop();
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
        public void TestAddPlayer(string playerName, int iconIndex)
        {
            var clientPlayer = ClientInstance.World.EntityManager.GetSingleton<PlayerSubmitGlobalComponent>();
            var message = new SubmitData()
            {
                Type = PlayerSubmitGlobalMessageType.CREATE_PLAYER,
                Arg0 = ClientInstance.World.PlayerId.Id,
                Arg1 = iconIndex,
                Data = Encoding.UTF8.GetBytes(playerName)
            };

            clientPlayer.AddSubmitMessage(message);
        }
    }
}