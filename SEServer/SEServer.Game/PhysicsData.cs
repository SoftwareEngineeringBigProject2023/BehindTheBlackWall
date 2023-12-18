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
    public Dictionary<EId, ContactRecordData> Contacts { get; set; } = new();

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
        BindBody.BodyType = rigidbodyComponent.BodyType switch
        {
            PhysicsBodyType.Static => BodyType.Static,
            PhysicsBodyType.Kinematic => BodyType.Kinematic,
            PhysicsBodyType.Dynamic => BodyType.Dynamic,
            _ => BodyType.Static
        };
        
        UpdateBody(rigidbodyComponent);
    }
    
    public void SetIsSensor(bool isSensor)
    {
        foreach (var fixture in BindBody.FixtureList)
        {
            if (fixture != null)
            {
                fixture.IsSensor = isSensor;
            }
        }
    }
    
    /// <summary>
    /// 更新物理数据
    /// </summary>
    /// <param name="rigidbodyComponent"></param>
    public void UpdateBody(RigidbodyComponent rigidbodyComponent)
    {
        if (rigidbodyComponent.SetPosition != null)
        {
            BindBody.Position = rigidbodyComponent.SetPosition.Value.ToPhysicsVector2();
            rigidbodyComponent.SetPosition = null;
        }
        
        if (rigidbodyComponent.SetVelocity != null)
        {
            BindBody.LinearVelocity = rigidbodyComponent.SetVelocity.Value.ToPhysicsVector2();
            rigidbodyComponent.SetVelocity = null;
        }
        
        if (rigidbodyComponent.SetRotation != null)
        {
            BindBody.Rotation = rigidbodyComponent.SetRotation.Value;
            rigidbodyComponent.SetRotation = null;
        }
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