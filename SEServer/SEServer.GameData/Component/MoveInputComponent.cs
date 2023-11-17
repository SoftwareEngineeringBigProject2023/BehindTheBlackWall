using MessagePack;
using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData.Component;

/// <summary>
/// 移动输入组件
/// </summary>
[MessagePackObject]
public class MoveInputComponent : IC2SComponent
{
    [Key(0)]
    public CId Id { get; set; }
    [Key(1)]
    public EId EntityId { get; set; }

    [Key(2)]
    public PlayerId Owner { get; set; }
    /// <summary>
    /// 移动输入组件
    /// </summary>
    [Key(3)]
    public SVector2 Input { get; set; }
    /// <summary>
    /// 瞄准角度
    /// </summary>
    [Key(4)]
    public float TargetRotation { get; set; }
}