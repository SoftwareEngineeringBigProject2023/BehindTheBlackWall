using System.Numerics;
using MessagePack;
using SEServer.Data;

namespace SEServer.GameData;

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
    [IgnoreMember]
    public bool IsDirty { get; set; }
    [Key(2)]
    public PlayerId Owner { get; set; }
    /// <summary>
    /// 移动输入组件
    /// </summary>
    [Key(3)]
    public SVector2 Input { get; set; }
}