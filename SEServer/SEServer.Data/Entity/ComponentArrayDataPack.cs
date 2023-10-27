using MessagePack;

namespace SEServer.Data;

/// <summary>
/// 组件表数据包
/// </summary>
[MessagePackObject]
public class ComponentArrayDataPack
{
    [Key(0)]
    public int TypeCode { get; set; }
    [Key(1)]
    public byte[] Data { get; set; } = null!;
}