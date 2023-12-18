using SEServer.Data.Message;
using SEServer.GameData.CTData;

namespace SEServer.GameData.Builder;

public class UnitShootNotifyGlobalMessageBuilder
{
    private int _entityId;
    private int _weaponId;
    
    public UnitShootNotifyGlobalMessageBuilder SetEntityId(int entityId)
    {
        _entityId = entityId;
        return this;
    }
    
    public UnitShootNotifyGlobalMessageBuilder SetWeaponId(int weaponId)
    {
        _weaponId = weaponId;
        return this;
    }

    public NotifyData Build()
    {
        return new NotifyData()
        {
            Type = PlayerNotifyGlobalMessageType.PLAYER_SHOOT,
            Arg0 = _entityId,
            Arg1 = _weaponId
        };
    }
}