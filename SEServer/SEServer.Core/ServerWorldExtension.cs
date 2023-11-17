using SEServer.Data;
using SEServer.Data.Message;
using SEServer.GameData;
using SEServer.GameData.Component;

namespace SEServer.Core;

public static class ServerWorldExtension
{
    public static void SendPlayerMessage(this World? world, SubmitData submitData)
    {
        if(world == null)
            return;

        var playerComponent = world.EntityManager.GetSingleton<PlayerSubmitGlobalComponent>();
        playerComponent.AddSubmitMessage(submitData);
    }
}