using SEServer.Data.Message;

namespace SEServer.GameData.Builder;

public class UnitInjuryNotifyGlobalMessageBuilder
{
    private int _entityId;
    private int _damage;
    
    public UnitInjuryNotifyGlobalMessageBuilder SetEntityId(int entityId)
    {
        _entityId = entityId;
        return this;
    }
    
    public UnitInjuryNotifyGlobalMessageBuilder SetDamage(int damage)
    {
        _damage = damage;
        return this;
    }

    public NotifyData Build()
    {
        return new NotifyData()
        {
            Type = PlayerNotifyGlobalMessageType.PLAYER_INJURY,
            Arg0 = _entityId,
            Arg1 = _damage
        };
    }
}