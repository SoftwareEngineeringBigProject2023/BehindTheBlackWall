using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData.Component;

public class RigidbodyComponent : IComponent
{
    public CId Id { get; set; }
    public EId EntityId { get; set; }
    /// <summary>
    /// 刚体类型
    /// </summary>
    public PhysicsBodyType BodyType { get; set; }
    /// <summary>
    /// 是否是触发器（不参与物理碰撞）
    /// </summary>
    public bool IsTrigger { get; set; } = false;
    /// <summary>
    /// 质量
    /// </summary>
    public float Mass { get; set; } = 1f;
    /// <summary>
    /// 线性阻尼
    /// </summary>
    public float LinearDamping { get; set; } = 0f;
    /// <summary>
    /// 角速度阻尼
    /// </summary>
    public float AngularDamping { get; set; } = 0.01f;
    /// <summary>
    /// 是否固定旋转
    /// </summary>
    public bool IsFixedRotation { get; set; } = false;
    /// <summary>
    /// 是否属于子弹 —— 使用连续碰撞检测
    /// </summary>
    public bool IsBullet { get; set; } = false;
    /// <summary>
    /// 当前的速度
    /// </summary>
    public SVector2 Velocity { get; set; } = SVector2.Zero;
    /// <summary>
    /// 当前的旋转角度
    /// </summary>
    public float Rotation { get; set; } = 0;
    /// <summary>
    /// 当前设置的速度
    /// </summary>
    public SVector2 SetVelocity { get; set; } = SVector2.Invalid;
    /// <summary>
    /// 当前设置的旋转角度
    /// </summary>
    public float? SetRotation { get; set; } = 0;
}