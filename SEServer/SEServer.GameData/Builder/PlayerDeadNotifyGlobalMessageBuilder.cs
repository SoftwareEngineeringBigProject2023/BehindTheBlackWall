using SEServer.Data.Message;

namespace SEServer.GameData.Builder;

public class PlayerDeadNotifyGlobalMessageBuilder
{
    private int _playerId;

    public PlayerDeadNotifyGlobalMessageBuilder SetPlayerId(int playerId)
    {
        _playerId = playerId;
        return this;
    }
    

    public NotifyData Build()
    {
        return new NotifyData()
        {
            Type = PlayerNotifyGlobalMessageType.PLAYER_DEAD,
            Arg0 = _playerId
        };
    }
}