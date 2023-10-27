using System.Numerics;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.Data.Message;
using SEServer.GameData;

namespace SEServer.Game;

/// <summary>
/// 玩家系统，负责处理玩家与实体的绑定
/// </summary>
[Priority(50)]
public class PlayerSystem : ISystem
{
    public World World { get; set; }
    public ServerWorld ServerWorld => (ServerWorld)World;
    public void Init()
    {
        ServerWorld.EntityManager.GetSingleton<PlayerMessageComponent>();
    }

    public void Update()
    {
        var playerMessages = ServerWorld.EntityManager.GetSingleton<PlayerMessageComponent>();
        var playerCollection = ServerWorld.EntityManager.GetComponentDataCollection<PlayerComponent>();
        
        while (playerMessages.SubmitMessages.TryDequeue(out var message))
        {
            switch (message.Type)
            {
                case PlayerSubmitMessageType.CREATE_PLAYER:
                    HandleCreatePlayer(message);
                    break;
                case PlayerSubmitMessageType.DESTROY_PLAYER:
                    HandleDestroyPlayer(message, playerCollection);
                    break;
            }
        }
    }

    private void HandleCreatePlayer(SubmitData message)
    {
        var playerId = message.Arg0;
        
        var entity = World.EntityManager.CreateEntity();
        var playerCom = World.EntityManager.GetOrAddComponent<PlayerComponent>(entity);
        playerCom.PlayerId = playerId;
        
        var inputCom = World.EntityManager.GetOrAddComponent<MoveInputComponent>(entity);
        inputCom.Owner = new PlayerId() { Id = playerId };
        inputCom.Input = Vector2.Zero.ToSVector2();
        
        var transformCom = World.EntityManager.GetOrAddComponent<TransformComponent>(entity);
        transformCom.Position = Vector2.Zero.ToSVector2();
        
        // 刚体
        var rigidbodyCom = World.EntityManager.GetOrAddComponent<RigidbodyComponent>(entity);
        rigidbodyCom.BodyType = PhysicsBodyType.Dynamic;
        rigidbodyCom.IsFixedRotation = true;
        
        // 图形
        var shapeComponent = World.EntityManager.GetOrAddComponent<ShapeComponent>(entity);
        shapeComponent.Shapes.Add(new CircleShapeData(0.5f));
        
        var propertyCom = World.EntityManager.GetOrAddComponent<PropertyComponent>(entity);
        propertyCom.Speed = 5;
        
        World.EntityManager.GetOrAddComponent<PlayerNotifyComponent>(entity);

        World.EntityManager.GetOrAddComponent<GraphComponent>(entity);
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
}