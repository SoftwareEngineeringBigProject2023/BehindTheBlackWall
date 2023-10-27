using MessagePack;
using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData;

public class RigidbodyComponent : IComponent
{
    public CId Id { get; set; }
    public EId EntityId { get; set; }
    public PhysicsBodyType BodyType { get; set; }
    public float Mass { get; set; } = 1f;
    public float LinearDamping { get; set; } = 0f;
    public float AngularDamping { get; set; } = 0.01f;
    public bool IsFixedRotation { get; set; } = false;
    public bool IsBullet { get; set; } = false;
    public SVector2 Velocity { get; set; } = SVector2.Zero;
    public SVector2 SetVelocity { get; set; } = SVector2.Invalid;
}