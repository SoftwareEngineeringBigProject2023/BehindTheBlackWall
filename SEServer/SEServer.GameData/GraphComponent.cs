using MessagePack;
using SEServer.Data;

namespace SEServer.GameData;

[MessagePackObject]
public class GraphComponent : IS2CComponent
{
    [Key(0)]
    public CId Id { get; set; }
    [Key(1)]
    public EId EntityId { get; set; }
    [IgnoreMember]
    public bool IsDirty { get; set; }
}