using MessagePack;
using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData.Component;

[MessagePackObject]
public class ShootInputComponent : IC2SComponent
{
    [Key(0)]
    public CId Id { get; set; }
    [Key(1)]
    public EId EntityId { get; set; }
    [Key(2)]
    public PlayerId Owner { get; set; }
    [Key(3)]
    public bool TriggerShoot { get; set; }
}