using MessagePack;

namespace SEServer.Data;

[MessagePackObject]
public class Entity
{
    [Key(0)]
    public EId Id { get; set; }
    public static Entity Empty { get; } = new Entity { Id = EId.Invalid };
}