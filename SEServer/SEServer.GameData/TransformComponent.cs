using System.Numerics;
using MessagePack;
using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData;

[MessagePackObject]
public class TransformComponent : IS2CComponent
{
    [Key(0)]
    public CId Id { get; set; }
    [Key(1)]
    public EId EntityId { get; set; }
    /// <summary>
    /// 坐标位置
    /// </summary>
    [Key(2)]
    public SVector2 Position { get; set; }
    /// <summary>
    /// 角度 角度制
    /// </summary>
    [Key(3)]
    public float Rotation { get; set; }
}