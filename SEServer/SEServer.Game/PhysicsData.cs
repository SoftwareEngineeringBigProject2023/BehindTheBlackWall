using nkast.Aether.Physics2D.Dynamics;
using SEServer.Data;
using SEServer.GameData;
using World = SEServer.Data.World;

namespace SEServer.Game;

/// <summary>
/// 模拟的物理数据
/// </summary>
public class PhysicsData
{
    public EId BindEntityId { get; set; }
    public PhysicsBodyType PBodyType { get; set; }
    public Body BindBody { get; set; } = null!;
    public Dictionary<int, Fixture> Shapes { get; set; } = new();

    public void SetBodyInfo(RigidbodyComponent rigidbodyComponent)
    {
        BindBody.Mass = rigidbodyComponent.Mass;
        BindBody.LinearDamping = rigidbodyComponent.LinearDamping;
        BindBody.AngularDamping = rigidbodyComponent.AngularDamping;
        BindBody.FixedRotation = rigidbodyComponent.IsFixedRotation;
        BindBody.IsBullet = rigidbodyComponent.IsBullet;
        BindBody.BodyType = rigidbodyComponent.BodyType switch
        {
            PhysicsBodyType.Static => BodyType.Static,
            PhysicsBodyType.Kinematic => BodyType.Kinematic,
            PhysicsBodyType.Dynamic => BodyType.Dynamic,
            _ => BodyType.Static
        };

        if (rigidbodyComponent.SetVelocity != SVector2.Invalid)
            BindBody.LinearVelocity = rigidbodyComponent.SetVelocity.ToPhysicsVector2();
    }

    public void UpdateInfo(World world,RigidbodyComponent rigidbodyComponent, TransformComponent transformComponent)
    {
        var velocity = BindBody.LinearVelocity.ToSVector2();
        if(velocity != rigidbodyComponent.Velocity)
        {
            rigidbodyComponent.Velocity = velocity;
            world.EntityManager.MarkAsChanged(rigidbodyComponent);
        }
        var position = BindBody.Position.ToSVector2();
        if(position != transformComponent.Position)
        {
            transformComponent.Position = position;
            world.EntityManager.MarkAsDirty(transformComponent);
        }
    }

    public void SetPosition(SVector2 position)
    {
        BindBody.Position = position.ToPhysicsVector2();
    }
}