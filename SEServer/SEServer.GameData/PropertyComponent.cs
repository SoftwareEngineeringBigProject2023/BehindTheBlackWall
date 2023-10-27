using MessagePack;
using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData;

[MessagePackObject]
public class PropertyComponent : IS2CComponent
{
    [Key(0)]
    public CId Id { get; set; }
    [Key(1)]
    public EId EntityId { get; set; }
    [Key(2)]
    public float Speed { get; set; } = 5f;
    [Key(3)]
    public SVector2 LineVelocity { get; set; } = SVector2.Zero;
}