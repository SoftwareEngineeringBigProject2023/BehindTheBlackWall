using MessagePack;

namespace SEServer.Data;

[MessagePackObject]
public class Entity
{
    [Key(0)]
    public EId Id { get; set; }
    public static Entity Empty { get; } = new Entity { Id = EId.Empty };
    /// <summary>
    /// 实体的脏标记
    /// 只有当组件发生数量与类型变化时，才会被标记为脏
    /// </summary>

}