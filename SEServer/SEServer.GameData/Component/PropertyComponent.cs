using MessagePack;
using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData.Component;

public class PropertyComponent : IComponent
{
    public CId Id { get; set; }
    public EId EntityId { get; set; }
    public string Name { get; set; } = "";
    public float Speed { get; set; } = 5f;
    /// <summary>
    /// 瞄准角度
    /// </summary>
    public float TargetAimRotation { get; set; }
    /// <summary>
    /// 最大生命值
    /// </summary>
    public float HpMax { get; set; } = 100f;
    /// <summary>
    /// 当前生命值
    /// </summary>
    public float Hp { get; set; } = 100f;
    /// <summary>
    /// 受伤时间统计，5秒没受伤则恢复
    /// </summary>
    public double HeartTimer { get; set; }
}