using MessagePack;
using SEServer.Data;

namespace SEServer.GameData;

[MessagePackObject]
public class PlayerComponent : IS2CComponent
{
    [Key(0)]
    public CId Id { get; set; }
    [Key(1)]
    public EId EntityId { get; set; }
    [IgnoreMember]
    public bool IsDirty { get; set; }
    [Key(2)]
    public int PlayerId { get; set; }
}