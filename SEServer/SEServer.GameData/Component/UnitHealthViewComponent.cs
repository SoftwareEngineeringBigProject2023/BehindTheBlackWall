using MessagePack;
using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData.Component;

[MessagePackObject]
public class UnitHealthViewComponent : IS2CComponent
{
    [Key(0)]
    public CId Id { get; set; }
    [Key(1)]
    public EId EntityId { get; set; }
    
    /// <summary>
    /// 最大生命值
    /// </summary>
    [Key(2)]
    public float HpMax { get; set; } = 100f;
    /// <summary>
    /// 当前生命值
    /// </summary>
    [Key(3)]
    public float Hp { get; set; } = 100f;
}