using System.Collections.Generic;
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
    public List<byte[]> Data { get; set; } = new();
    [IgnoreMember]
    public string? DebugName { get; set; }
}