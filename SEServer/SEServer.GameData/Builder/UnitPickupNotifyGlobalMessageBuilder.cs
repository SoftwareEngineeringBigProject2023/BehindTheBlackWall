using SEServer.Data.Message;

namespace SEServer.GameData.Builder;

public class UnitPickupNotifyGlobalMessageBuilder
{
    private int _entityId;

    public UnitPickupNotifyGlobalMessageBuilder SetEntityId(int entityId)
    {
        _entityId = entityId;
        return this;
    }

    public NotifyData Build()
    {
        return new NotifyData()
        {
            Type = PlayerNotifyGlobalMessageType.PLAYER_PICKUP,
        };
    }
}