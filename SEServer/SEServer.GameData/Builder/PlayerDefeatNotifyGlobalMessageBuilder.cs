using SEServer.Data.Message;

namespace SEServer.GameData.Builder;

public class PlayerDefeatNotifyGlobalMessageBuilder
{
    private int _playerId;
    private int _defeatedPlayerId;

    public PlayerDefeatNotifyGlobalMessageBuilder SetPlayerId(int entityIdId)
    {
        _playerId = entityIdId;
        return this;
    }
    
    public PlayerDefeatNotifyGlobalMessageBuilder SetDefeatedId(int defeatedPlayerId)
    {
        _defeatedPlayerId = defeatedPlayerId;
        return this;
    }
    
    public NotifyData Build()
    {
        return new NotifyData()
        {
            Type = PlayerNotifyGlobalMessageType.PLAYER_DEFEAT,
            Arg0 = _playerId,
            Arg1 = _defeatedPlayerId
        };
    }
}