using System.Numerics;
using SEServer.Data;
using SEServer.Data.Message;
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
        
        var tagPlayer = componentDataCollection.Components.FirstOrDefault(p => p.PlayerId == playerId);
        if (tagPlayer != null)
        {
            // 重复创建
            return;
        }

        var entity = World.EntityManager.CreateEntity();
        var playerCom = World.EntityManager.GetOrAddComponent<PlayerComponent>(entity);
        playerCom.PlayerId = playerId;
        
        var graphCom = World.EntityManager.GetOrAddComponent<GraphComponent>(entity);
        graphCom.Type = GraphType.Unit;
        
        var inputCom = World.EntityManager.GetOrAddComponent<MoveInputComponent>(entity);
        inputCom.Owner = new PlayerId() { Id = playerId };
        inputCom.Input = Vector2.Zero.ToSVector2();
        
        var shootCom = World.EntityManager.GetOrAddComponent<ShootInputComponent>(entity);
        shootCom.Owner = new PlayerId() { Id = playerId };

        var transformCom = World.EntityManager.GetOrAddComponent<TransformComponent>(entity);
        transformCom.Position = Vector2.Zero.ToSVector2();
        
        // 刚体
        var rigidbodyCom = World.EntityManager.GetOrAddComponent<RigidbodyComponent>(entity);
        rigidbodyCom.BodyType = PhysicsBodyType.Dynamic;
        rigidbodyCom.IsFixedRotation = true;
        
        // 图形
        var shapeComponent = World.EntityManager.GetOrAddComponent<ShapeComponent>(entity);
        shapeComponent.Shapes.Add(new CircleShapeData(0.5f));
        
        // 属性
        var propertyCom = World.EntityManager.GetOrAddComponent<PropertyComponent>(entity);
        propertyCom.Speed = 5;
        
        // 武器
        var weaponCom = World.EntityManager.GetOrAddComponent<WeaponComponent>(entity);
        weaponCom.WeaponShootCooldown = 0;

        var notifyComponent = World.EntityManager.GetOrAddComponent<PlayerNotifyComponent>(entity);
        notifyComponent.Owner = new PlayerId() { Id = playerId };
        notifyComponent.AddNotifyMessage(new PlayerInitNotifyGlobalMessageBuilder()
            .SetPlayerEntity(playerId)
            .Build());
        
        var submitComponent = World.EntityManager.GetOrAddComponent<PlayerSubmitComponent>(entity);
        submitComponent.Owner = new PlayerId() { Id = playerId };
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