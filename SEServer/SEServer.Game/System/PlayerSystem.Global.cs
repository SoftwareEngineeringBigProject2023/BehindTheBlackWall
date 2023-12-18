using System.Numerics;
using System.Text;
using SEServer.Data;
using SEServer.Data.Message;
using SEServer.Game.Component;
using SEServer.GameData;
using SEServer.GameData.Builder;
using SEServer.GameData.Component;
using SEServer.GameData.Data;

namespace SEServer.Game.System;

public partial class PlayerSystem
{
    private void HandleCreatePlayer(SubmitData message,
        ComponentDataCollection<PlayerComponent> componentDataCollection)
    {
        var playerId = message.Arg0;
        var playerIcon = message.Arg1;
        var playerName = Encoding.UTF8.GetString(message.Data);
        
        var tagPlayer = componentDataCollection.Components.FirstOrDefault(p => p.PlayerId == playerId);
        if (tagPlayer != null)
        {
            // 重复创建
            return;
        }

        var entity = World.EntityManager.CreateEntity();
        // 玩家
        var playerCom = World.EntityManager.GetOrAddComponent<PlayerComponent>(entity);
        playerCom.PlayerId = playerId;
        
        AddDisplayComponents(entity, playerId, playerIcon);
        AddInputComponents(entity, playerId);
        AddPhysicsComponents(entity, playerId);
        AddDataComponents(entity, playerId, playerName);
        AddViewComponents(entity, playerId);
        AddMessageComponents(entity, playerId);
        
        RandomSpawn(entity, playerId);

        var globalNotifyComponent = World.EntityManager.GetSingleton<PlayerNotifyGlobalComponent>();
        globalNotifyComponent.AddNotifyMessage(new PlayerInitNotifyGlobalMessageBuilder()
            .SetPlayerId(playerId)
            .SetPlayerEntity(entity.Id.Id)
            .Build());
    }
    
    private void AddDisplayComponents(Entity entity, int playerId, int playerIcon)
    {
        // 图形
        var graphCom = World.EntityManager.GetOrAddComponent<GraphComponent>(entity);
        graphCom.Type = GraphType.Unit;
        graphCom.Res = playerIcon;
        
        // 位移
        var transformCom = World.EntityManager.GetOrAddComponent<TransformComponent>(entity);
        transformCom.Position = Vector2.Zero.ToSVector2();
    }
    
    private void AddInputComponents(Entity entity, int playerId)
    {
        // 移动输入
        var inputCom = World.EntityManager.GetOrAddComponent<MoveInputComponent>(entity);
        inputCom.Owner = new PlayerId() { Id = playerId };
        inputCom.Input = Vector2.Zero.ToSVector2();
        
        // 射击输入
        var shootCom = World.EntityManager.GetOrAddComponent<ShootInputComponent>(entity);
        shootCom.Owner = new PlayerId() { Id = playerId };
        
        // 交互输入
        var interactCom = World.EntityManager.GetOrAddComponent<InteractInputComponent>(entity);
        interactCom.Owner = new PlayerId() { Id = playerId };
        interactCom.SelectedWeaponIndex = 0;
    }

    private void AddPhysicsComponents(Entity entity, int playerId)
    {
        // 物理刚体
        var rigidbodyCom = World.EntityManager.GetOrAddComponent<RigidbodyComponent>(entity);
        rigidbodyCom.BodyType = PhysicsBodyType.Dynamic;
        rigidbodyCom.IsFixedRotation = true;
        rigidbodyCom.Tag = PhysicsTag.UNIT;
        
        // 物理形状
        var shapeComponent = World.EntityManager.GetOrAddComponent<ShapeComponent>(entity);
        shapeComponent.Shapes.Add(new CircleShapeData(0.5f));
    }
    
    private void AddDataComponents(Entity entity, int playerId, string playerName)
    {
        // 属性
        var propertyCom = World.EntityManager.GetOrAddComponent<PropertyComponent>(entity);
        propertyCom.Speed = 5;
        propertyCom.Name = playerName;
        propertyCom.HpMax = 50;
        propertyCom.Hp = 50;
        
        // 武器
        var weaponCom = World.EntityManager.GetOrAddComponent<WeaponComponent>(entity);

        // 背包
        var bagCom = World.EntityManager.GetOrAddComponent<BagComponent>(entity);
        bagCom.SelectedWeaponIndex = 0;
        bagCom.NeedRefresh = true;
        // 设置测试武器
        bagCom.Weapons.Add(new BagItem()
        {
            BindType = BagItemType.Weapon,
            BindId = "Gun_1",
        });
        bagCom.Weapons.Add(new BagItem()
        {
            BindType = BagItemType.Weapon,
            BindId = "Gun_2",
        });
        bagCom.Weapons.Add(new BagItem()
        {
            BindType = BagItemType.Weapon,
            BindId = "Gun_3",
        });
    }
    
    private void AddViewComponents(Entity entity, int playerId)
    {
        var unitHealthViewCom = World.EntityManager.GetOrAddComponent<UnitHealthViewComponent>(entity);
        
        var unitAimViewCom = World.EntityManager.GetOrAddComponent<UnitAimViewComponent>(entity);

        var scoreVieCom = World.EntityManager.GetOrAddComponent<ScoreViewComponent>(entity);
    }
    
    private void AddMessageComponents(Entity entity, int playerId)
    {
        var notifyComponent = World.EntityManager.GetOrAddComponent<PlayerNotifyComponent>(entity);
        notifyComponent.Owner = new PlayerId() { Id = playerId };

        var submitComponent = World.EntityManager.GetOrAddComponent<PlayerSubmitComponent>(entity);
        submitComponent.Owner = new PlayerId() { Id = playerId };
    }
    
    private void RandomSpawn(Entity entity, int playerId)
    {
        var mapComponent = World.EntityManager.GetSingleton<MapDataSingletonComponent>();
        var mapData = mapComponent.MapCTData;
        
        var random = new Random();
        var randomIndex = random.Next(0, mapData.SpawnPoints.Count);
        var spawnPoint = mapData.SpawnPoints[randomIndex];
        
        var transformCom = World.EntityManager.GetComponent<TransformComponent>(entity);
        if (transformCom == null)
        {
            return;
        }
        
        transformCom.Position = spawnPoint.Position;
    }


    private void HandleDestroyPlayer(SubmitData message,
        ComponentDataCollection<PlayerComponent> componentDataCollection)
    {
        var playerId = message.Arg0;
        var tagPlayer = componentDataCollection.Components.FirstOrDefault(p => p.PlayerId == playerId);
        if (tagPlayer != null)
        {
            World.EntityManager.RemoveEntity(tagPlayer.EntityId);
        }
    }
    
    private void HandlePlayerExit(SubmitData message, ComponentDataCollection<PlayerComponent> playerCollection)
    {
        var playerId = message.Arg0;
        var tagPlayer = playerCollection.Components.FirstOrDefault(p => p.PlayerId == playerId);
        if (tagPlayer != null)
        {
            World.EntityManager.RemoveEntity(tagPlayer.EntityId);
        }
    }
}