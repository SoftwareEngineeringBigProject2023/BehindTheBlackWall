using SEServer.Data.Message;

namespace SEServer.GameData.Builder;

public class PlayerInitNotifyGlobalMessageBuilder
{
    private int _playerEntityId;
    
    public PlayerInitNotifyGlobalMessageBuilder SetPlayerEntity(int entityId)
    {
        _playerEntityId = entityId;
        return this;
    }

    public NotifyData Build()
    {
        return new NotifyData()
        {
            Type = PlayerNotifyGlobalMessageType.PLAYER_INIT,
            Arg0 = _playerEntityId
        };
    }
}