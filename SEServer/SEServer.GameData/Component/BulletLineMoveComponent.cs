using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData.Component;

public class BulletLineMoveComponent : IComponent
{
    public CId Id { get; set; }
    public EId EntityId { get; set; }
    public SVector2 Direction { get; set; }
    public float Speed { get; set; }
    public float Distance { get; set; }
    public float MaxDistance { get; set; }
}