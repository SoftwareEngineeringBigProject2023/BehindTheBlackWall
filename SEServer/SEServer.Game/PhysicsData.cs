using nkast.Aether.Physics2D.Dynamics;
using SEServer.Data;
using SEServer.GameData;
using SEServer.GameData.Component;
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
    /// <summary>
    /// 当前连接的物理数据
    /// </summary>
    public List<PhysicsData> Contacts { get; set; } = new();

    /// <summary>
    /// 初始化物理数据
    /// </summary>
    /// <param name="rigidbodyComponent"></param>
    public void InitBody(RigidbodyComponent rigidbodyComponent)
    {
        BindBody.Mass = rigidbodyComponent.Mass;
        BindBody.LinearDamping = rigidbodyComponent.LinearDamping;
        BindBody.AngularDamping = rigidbodyComponent.AngularDamping;
        BindBody.FixedRotation = rigidbodyComponent.IsFixedRotation;
        BindBody.IsBullet = rigidbodyComponent.IsBullet;
        foreach (var fixture in BindBody.FixtureList)
        {
            if (fixture != null)
            {
                fixture.IsSensor = rigidbodyComponent.IsTrigger;
            }
        }
        BindBody.BodyType = rigidbodyComponent.BodyType switch
        {
            PhysicsBodyType.Static => BodyType.Static,
            PhysicsBodyType.Kinematic => BodyType.Kinematic,
            PhysicsBodyType.Dynamic => BodyType.Dynamic,
            _ => BodyType.Static
        };
        
        UpdateBody(rigidbodyComponent);
    }
    
    /// <summary>
    /// 更新物理数据
    /// </summary>
    /// <param name="rigidbodyComponent"></param>
    public void UpdateBody(RigidbodyComponent rigidbodyComponent)
    {
        if (rigidbodyComponent.SetVelocity != SVector2.Invalid)
            BindBody.LinearVelocity = rigidbodyComponent.SetVelocity.ToPhysicsVector2();
        
        if (rigidbodyComponent.SetRotation != null)
            BindBody.Rotation = rigidbodyComponent.SetRotation.Value;
    }

    public void UpdateInfo(World world, RigidbodyComponent rigidbodyComponent, TransformComponent transformComponent)
    {
        // 更新速度
        var velocity = BindBody.LinearVelocity.ToSVector2();
        if(velocity != rigidbodyComponent.Velocity)
        {
            rigidbodyComponent.Velocity = velocity;
            world.EntityManager.MarkAsChanged(rigidbodyComponent);
        }
        
        // 更新移动
        var position = BindBody.Position.ToSVector2();
        if(position != transformComponent.Position)
        {
            transformComponent.Position = position;
            world.EntityManager.MarkAsDirty(transformComponent);
        }
        
        // 更新旋转
        var rotation = BindBody.Rotation;
        if(Math.Abs(rotation - transformComponent.Rotation) > 0.05f)
        {
            transformComponent.Rotation = rotation;
            world.EntityManager.MarkAsDirty(transformComponent);
        }
    }

    public void SetPosition(SVector2 position)
    {
        BindBody.Position = position.ToPhysicsVector2();
    }
}