using nkast.Aether.Physics2D.Collision;
using nkast.Aether.Physics2D.Collision.Shapes;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData.Component;
using Phy2DWorld = nkast.Aether.Physics2D.Dynamics.World;
using World = SEServer.Data.World;

namespace SEServer.Game.System;

[Priority(20)]
public class BulletSystem : ISystem
{
    public World World { get; set; } = null!;
    public void Init()
    {
        
    }

    public void Update()
    {
        var bulletComponentCollection = World.EntityManager.GetComponentDataCollection<BulletComponent, RigidbodyComponent>();
        var physicsSingletonComponent = World.EntityManager.GetSingleton<PhysicsSingletonComponent>();
        
        foreach (var tuple in bulletComponentCollection)
        {
            var (bulletComponent, rigidbodyComponent) = tuple;
            MoveBullet(bulletComponent, rigidbodyComponent);
            
            CheckBulletCollision(bulletComponent, rigidbodyComponent, physicsSingletonComponent);
        }
        
    }

    private void CheckBulletCollision(BulletComponent bullet, RigidbodyComponent rigidbody, 
        PhysicsSingletonComponent physicsSingleton)
    {
        var bulletPhyData = physicsSingleton.GetPhysicsData(bullet.EntityId);
        if (bulletPhyData == null)
        {
            return;
        }

        foreach (var otherPhyData in bulletPhyData.Contacts)
        {
            var property = World.EntityManager.GetComponent<PropertyComponent>(otherPhyData.BindEntityId);
            if (property == null)
            {
                continue;
            }

            if (bullet.CreatorId != property.EntityId.Id)
            {
                World.EntityManager.Entities.MarkAsToBeDelete(bullet.EntityId);
                property.Hp -= bullet.Damage;
                World.EntityManager.MarkAsDirty(property);
                
                var tagRigidbody = World.EntityManager.GetComponent<RigidbodyComponent>(otherPhyData.BindEntityId);
                var curTransform = World.EntityManager.GetComponent<TransformComponent>(bullet.EntityId);
                if (tagRigidbody == null || curTransform == null)
                {
                    continue;
                }

                var direction = SVector2.FromAngle(curTransform.Rotation);
                physicsSingleton.AddForce(tagRigidbody.EntityId, direction.normalized * 200f);
            }
        }
    }

    private void MoveBullet(BulletComponent bulletComponent, RigidbodyComponent rigidbodyComponent)
    {
        var bulletLineMoveComponent = World.EntityManager.GetComponent<BulletLineMoveComponent>(bulletComponent.EntityId);
        if (bulletLineMoveComponent != null)
        {
            MoveBulletLine(bulletComponent, rigidbodyComponent, bulletLineMoveComponent);
            return;
        }
    }

    private void MoveBulletLine(BulletComponent bulletComponent, RigidbodyComponent rigidbodyComponent, 
        BulletLineMoveComponent bulletLineMoveComponent)
    {
        var moveDistance = bulletLineMoveComponent.Speed * World.Time.DeltaTime;
        var moveVector = bulletLineMoveComponent.Direction * moveDistance;
        bulletLineMoveComponent.Distance += moveDistance;
        if (bulletLineMoveComponent.Distance > bulletLineMoveComponent.MaxDistance)
        {
            World.EntityManager.Entities.MarkAsToBeDelete(bulletComponent.EntityId);
            return;
        }
        
        rigidbodyComponent.SetVelocity = moveVector * bulletLineMoveComponent.Speed;
        World.EntityManager.MarkAsChanged(rigidbodyComponent);
    }
}