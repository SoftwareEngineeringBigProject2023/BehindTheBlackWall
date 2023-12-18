using SEServer.Data;
using SEServer.GameData.Builder;
using SEServer.GameData.Component;

namespace SEServer.Game.System;

public partial class PlayerSystem
{
    public void CheckPlayersState()
    {
        var collection = World.EntityManager.GetComponentDataCollection<PlayerComponent>();

        var curTotalTime = World.Time.TotalTime;
        foreach (var playerComponent in collection)
        {
            var propertyComponent = World.EntityManager.GetComponent<PropertyComponent>(playerComponent.EntityId);
            if (propertyComponent == null)
            {
                continue;
            }

            if (propertyComponent.Hp <= 0)
            {
                var playerId = playerComponent.PlayerId;

                World.EntityManager.Entities.MarkAsToBeDelete(playerComponent.EntityId);
                var globalNotifyComponent = World.EntityManager.GetSingleton<PlayerNotifyGlobalComponent>();
                globalNotifyComponent.AddNotifyMessage(new PlayerDeadNotifyGlobalMessageBuilder()
                    .SetPlayerId(playerId)
                    .Build());
            }

            if (propertyComponent.Hp > 0)
            {
                if(propertyComponent.HeartTimer == 0 || curTotalTime - propertyComponent.HeartTimer > 5.0)
                {
                    propertyComponent.Hp += 1;
                    propertyComponent.HeartTimer = curTotalTime - 5.0 + 0.1f;
                    
                    propertyComponent.Hp = Math.Min(propertyComponent.Hp, propertyComponent.HpMax);
                }
            }
        }
    }
}