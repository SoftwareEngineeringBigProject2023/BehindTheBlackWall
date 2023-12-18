using SEServer.Data.Message;

namespace SEServer.GameData.Builder;

public class UnitScoreChangedNotifyGlobalMessageBuilder
{
    private int _entityId;
    private int _score;
    
    public UnitScoreChangedNotifyGlobalMessageBuilder SetEntityId(int entityId)
    {
        _entityId = entityId;
        return this;
    }
    
    public UnitScoreChangedNotifyGlobalMessageBuilder SetScore(int score)
    {
        _score = score;
        return this;
    }

    public NotifyData Build()
    {
        return new NotifyData()
        {
            Type = PlayerNotifyGlobalMessageType.PLAYER_SCORE_CHANGE,
            Arg0 = _entityId,
            Arg1 = _score
        };
    }
}