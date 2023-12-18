using System.Linq;
using Game.Component;
using Game.GameComponent;
using Game.SceneScript;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData;
using SEServer.GameData.Component;
using SEServer.GameData.CTData;

namespace Game.GameSystem
{
    [Priority(1000)]
    public class PlayerMessageHandleSystem : ISystem
    {
        public World World { get; set; }
        public void Init()
        {
            
        }

        public void Update()
        {
            HandleGlobalNotifyMessages();
        }

        private void HandleGlobalNotifyMessages()
        {
            var notifyMessage = World.EntityManager.GetSingletonIfExists<PlayerNotifyGlobalComponent>();
            if (notifyMessage == null)
            {
                return;
            }

            while (notifyMessage.NotifyMessages.TryDequeue(out var message))
            {
                switch (message.Type)
                {
                    case PlayerNotifyGlobalMessageType.PLAYER_INIT:
                        HandlePlayerInitMessage(message.Arg0, message.Arg1);
                        break;
                    case PlayerNotifyGlobalMessageType.PLAYER_DEAD:
                        HandlePlayerDeadMessage(message.Arg0);
                        break;
                    case PlayerNotifyGlobalMessageType.PLAYER_SHOOT:
                        HandleUnitShootMessage(message.Arg0, message.Arg1);
                        break;
                    case PlayerNotifyGlobalMessageType.PLAYER_INJURY:
                        HandleUnitInjuryMessage(message.Arg0, message.Arg1);
                        break;
                    case PlayerNotifyGlobalMessageType.PLAYER_PICKUP:
                        HandleUnitPickupMessage(message.Arg0);
                        break;
                    case PlayerNotifyGlobalMessageType.PLAYER_SCORE_CHANGE:
                        HandleUnitScoreChangeMessage(message.Arg0, message.Arg1);
                        break;
                }
            }
        }

        private void HandlePlayerInitMessage(int playerId, int playerEntityId)
        {
            var clientWorld = (ClientWorld)World;
            if (playerId != clientWorld.PlayerId.Id)
            {
                return;
            }
            
            World.ServerContainer.Get<ILogger>().LogInfo("玩家初始化消息");
            var localPlayerInfo = clientWorld.EntityManager.GetSingleton<LocalPlayerInfoSingletonComponent>();
            localPlayerInfo.PlayerEntityId = new EId()
            {
                Id = playerEntityId
            };
        }
        
        private void HandlePlayerDeadMessage(int playerId)
        {
            var clientWorld = (ClientWorld)World;
            if (playerId != clientWorld.PlayerId.Id)
            {
                return;
            }
            
            World.ServerContainer.Get<ILogger>().LogInfo("玩家死亡消息");
            var localPlayerInfo = clientWorld.EntityManager.GetSingleton<LocalPlayerInfoSingletonComponent>();
            localPlayerInfo.PlayerEntityId = EId.Invalid;

            GameTestScene.Instance.EndGame();
        }

        private void HandleUnitShootMessage(int entityId, int weaponId)
        {
            var transform = World.EntityManager.GetComponent<TransformComponent>(new EId(){Id = entityId});
            if (transform == null)
                return;
            
            var weaponCTData = World.ServerContainer.Get<IConfigTable>().GetDataByIndex<WeaponCTData>(weaponId);

            var graphRequestGlobalComponent = World.EntityManager.GetSingleton<GraphRequestGlobalComponent>();
            graphRequestGlobalComponent.Requests.Enqueue(new ShootRequest(transform, weaponCTData));
        }
        
        private void HandleUnitInjuryMessage(int entityId, int damage)
        {
            var transform = World.EntityManager.GetComponent<TransformComponent>(new EId(){Id = entityId});
            if (transform == null)
                return;
            
            var graphRequestGlobalComponent = World.EntityManager.GetSingleton<GraphRequestGlobalComponent>();
            graphRequestGlobalComponent.Requests.Enqueue(new InjuryRequest(transform, damage));
        }
        
        private void HandleUnitScoreChangeMessage(int entityId, int score)
        {
            var transform = World.EntityManager.GetComponent<TransformComponent>(new EId(){Id = entityId});
            if (transform == null)
                return;
            
            var graphRequestGlobalComponent = World.EntityManager.GetSingleton<GraphRequestGlobalComponent>();
            graphRequestGlobalComponent.Requests.Enqueue(new ScoreChangeRequest(transform, score));
        }

        private void HandleUnitPickupMessage(int entityId)
        {
            var transform = World.EntityManager.GetComponent<TransformComponent>(new EId(){Id = entityId});
            if (transform == null)
                return;
            
            var localPlayerInfo = World.EntityManager.GetSingleton<LocalPlayerInfoSingletonComponent>();
            if (localPlayerInfo.PlayerEntityId.Id != entityId)
            {
                return;
            }
            
            var graphRequestGlobalComponent = World.EntityManager.GetSingleton<GraphRequestGlobalComponent>();
            graphRequestGlobalComponent.Requests.Enqueue(new PickupRequest(transform));
        }

    }
}