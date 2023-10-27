using System.Collections.Generic;
using MessagePack;
using SEServer.Data;

namespace SEServer.GameData;

[MessagePackObject]
public class PlayerMessageComponent : ISubmitComponent
{
    [Key(0)]
    public CId Id { get; set; }
    [Key(1)]
    public EId EntityId { get; set; }
    [Key(2)]
    public bool IsDirty { get; set; }
    [Key(3)]
    public PlayerId Owner { get; set; } = PlayerId.Invalid;
    [Key(4)]
    public Queue<SubmitData> SubmitMessages { get; set; } = new();
}