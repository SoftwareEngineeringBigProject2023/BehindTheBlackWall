using System.Collections.Generic;
using System.Linq;
using MessagePack;

namespace SEServer.Data;

/// <summary>
/// 实体与组件的快照
/// </summary>
[MessagePackObject]
public class Snapshot
{
    [Key(0)]
    public WId WorldId { get; set; }
    [Key(1)]
    public List<Entity> Entities { get; set; } = new();
    [Key(2)]
    public List<ComponentArrayDataPack> ComponentArrayDataPacks { get; set; } = new();


    public void AddDataPack(ComponentArrayDataPack dataPack)
    {
        ComponentArrayDataPacks.Add(dataPack);
    }

    public ComponentArrayDataPack? GetDataPack(int code)
    {
        return ComponentArrayDataPacks.FirstOrDefault(dataPack => dataPack.TypeCode == code);
    }
}