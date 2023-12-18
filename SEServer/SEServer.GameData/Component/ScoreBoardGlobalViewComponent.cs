using System.Collections.Generic;
using MessagePack;
using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData.Component;

[MessagePackObject]
public class ScoreBoardGlobalDataComponent : IComponent
{
    [Key(0)]
    public CId Id { get; set; }
    [Key(1)]
    public EId EntityId { get; set; }
    [Key(2)]
    public int PlayerIdHash { get; set; }
}

[MessagePackObject]
public class ScoreBoardGlobalViewComponent : IS2CComponent
{
    [Key(0)]
    public CId Id { get; set; }
    [Key(1)]
    public EId EntityId { get; set; }
    [Key(2)]
    public List<ScoreData> ScoreData { get; set; } = new List<ScoreData>();
}

[MessagePackObject]
public class ScoreData
{
    [Key(0)]
    public string Name { get; set; } = "";
    
    [Key(1)]
    public int Score { get; set; } = 0;
    
    [IgnoreMember]
    public EId BindEntityId { get; set; } = EId.Invalid;
}