using System.Numerics;
using MessagePack;
using SEServer.Data;

namespace SEServer.GameData;

[MessagePackObject]
public class TransformComponent : IS2CComponent
{
    [Key(0)]
    public CId Id { get; set; }
    [Key(1)]
    public EId EntityId { get; set; }
    [IgnoreMember]
    public bool IsDirty { get; set; }
    [Key(2)]
    public SVector2 Position { get; set; }
}