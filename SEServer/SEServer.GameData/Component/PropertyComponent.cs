using MessagePack;
using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData.Component;

[MessagePackObject]
public class PropertyComponent : IS2CComponent
{
    [Key(0)]
    public CId Id { get; set; }
    [Key(1)]
    public EId EntityId { get; set; }
    [Key(2)]
    public float Speed { get; set; } = 5f;
    /// <summary>
    /// 瞄准角度
    /// </summary>
    [Key(3)]
    public float TargetAimRotation { get; set; }
    /// <summary>
    /// 当前生命值
    /// </summary>
    [Key(4)]
    public float Hp { get; set; } = 100f;
}