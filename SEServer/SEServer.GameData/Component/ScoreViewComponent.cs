using MessagePack;
using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData.Component;

[MessagePackObject]
public class ScoreViewComponent : IS2CComponent
{
    [Key(0)]
    public CId Id { get; set; }
    [Key(1)]
    public EId EntityId { get; set; }
    [Key(2)]
    public int Score { get; set; }
}