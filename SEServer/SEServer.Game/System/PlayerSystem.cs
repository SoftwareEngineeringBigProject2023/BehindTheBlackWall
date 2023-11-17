using System.Numerics;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.Data.Message;
using SEServer.GameData;
using SEServer.GameData.Builder;
using SEServer.GameData.Component;
using SEServer.GameData.Data;

namespace SEServer.Game.System;

/// <summary>
/// 玩家系统，负责处理玩家与实体的绑定
/// </summary>
[Priority(50)]
public partial class PlayerSystem : ISystem
{
    public World World { get; set; }
    public ServerWorld ServerWorld => (ServerWorld)World;
    public void Init()
    {
        ServerWorld.EntityManager.GetSingleton<PlayerSubmitGlobalComponent>();
    }

    public void Update()
    {
        var playerSubmitGlobalComponent = ServerWorld.EntityManager.GetSingleton<PlayerSubmitGlobalComponent>();
        var playerCollection = ServerWorld.EntityManager.GetComponentDataCollection<PlayerComponent>();
        
        // 全局消息
        while (playerSubmitGlobalComponent.SubmitMessages.TryDequeue(out var message))
        {
            switch (message.Type)
            {
                case PlayerSubmitGlobalMessageType.CREATE_PLAYER:
                    HandleCreatePlayer(message, playerCollection);
                    break;
                case PlayerSubmitGlobalMessageType.DESTROY_PLAYER:
                    HandleDestroyPlayer(message, playerCollection);
                    break;
                case PlayerSubmitGlobalMessageType.PLAYER_EXIT:
                    HandlePlayerExit(message, playerCollection);
                    break;
            }
        }

        // 玩家消息
        var playerSubmitCollection = ServerWorld.EntityManager.GetComponentDataCollection<PlayerSubmitComponent>();
        foreach (var playerSubmitComponent in playerSubmitCollection)
        {
            while(playerSubmitComponent.SubmitMessages.TryDequeue(out var message))
            {
                switch (message.Type)
                {
                    
                }
            }
        }
    }
}