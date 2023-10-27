using System.Numerics;
using SEServer.Data;
using SEServer.GameData;

namespace SEServer.Game;

/// <summary>
/// 玩家系统，负责处理玩家与实体的绑定
/// </summary>
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
        var playerCom = World.EntityManager.AddComponent<PlayerComponent>(entity);
        playerCom.PlayerId = playerId;
        
        var inputCom = World.EntityManager.AddComponent<MoveInputComponent>(entity);
        inputCom.Owner = new PlayerId() { Id = playerId };
        inputCom.Input = Vector2.Zero.ToSVector2();
        
        var transformCom = World.EntityManager.AddComponent<TransformComponent>(entity);
        transformCom.Position = Vector2.Zero.ToSVector2();
        
        World.EntityManager.AddComponent<PlayerNotifyComponent>(entity);

        World.EntityManager.AddComponent<GraphComponent>(entity);
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