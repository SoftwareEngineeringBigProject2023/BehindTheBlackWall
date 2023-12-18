using SEServer.Data.Message;

namespace SEServer.GameData.Builder;

public class PlayerInitNotifyGlobalMessageBuilder
{
    private int _playerId;
    private int _playerEntityId;
    
    public PlayerInitNotifyGlobalMessageBuilder SetPlayerId(int playerId)
    {
        _playerId = playerId;
        return this;
    }
    
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
            Arg0 = _playerId,
            Arg1 = _playerEntityId
        };
    }
}