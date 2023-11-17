using nkast.Aether.Physics2D.Collision.Shapes;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData;
using SEServer.GameData.Component;
using SEServer.GameData.Data;

namespace SEServer.Game.System;

public class ShootSystem : ISystem
{
    public World World { get; set; }
    public void Init()
    {
        
    }

    public void Update()
    {
        var attackCollection = World.EntityManager
            .GetComponentDataCollection<ShootInputComponent, WeaponComponent, PropertyComponent>();
        
        foreach (var tuple in attackCollection)
        {
            var (shootInputComponent, weaponComponent, propertyComponent) = tuple;

            if(weaponComponent.WeaponShootCooldown > 0)
                weaponComponent.WeaponShootCooldown -= World.Time.DeltaTime;
            
            if (shootInputComponent.TriggerShoot && weaponComponent.WeaponShootCooldown <= 0)
            {
                CreateBullet(weaponComponent, propertyComponent);
            }
            
            shootInputComponent.TriggerShoot = false;
        }
    }

    private void CreateBullet(WeaponComponent weaponComponent, PropertyComponent propertyComponent)
    {
        var ownerTransform = World.EntityManager.GetComponent<TransformComponent>(weaponComponent.EntityId);
        var initPos = SVector2.Zero;
        if(ownerTransform != null)
            initPos = ownerTransform.Position;
        
        var bulletEntity = World.EntityManager.CreateEntity();
        var bulletComponent = World.EntityManager.GetOrAddComponent<BulletComponent>(bulletEntity);
        bulletComponent.Damage = 5;
        
        var bulletLineMoveComponent = World.EntityManager.GetOrAddComponent<BulletLineMoveComponent>(bulletEntity);
        bulletLineMoveComponent.Speed = 30;
        bulletLineMoveComponent.Direction = SVector2.FromAngle(propertyComponent.TargetAimRotation);
        bulletLineMoveComponent.MaxDistance = 60;
        
        var transformComponent = World.EntityManager.GetOrAddComponent<TransformComponent>(bulletEntity);
        transformComponent.Position = initPos;

        var rigidbodyComponent = World.EntityManager.GetOrAddComponent<RigidbodyComponent>(bulletEntity);
        rigidbodyComponent.BodyType = PhysicsBodyType.Dynamic;
        rigidbodyComponent.IsBullet = true;
        rigidbodyComponent.IsTrigger = true;
        rigidbodyComponent.SetRotation = propertyComponent.TargetAimRotation;
        
        var shapeComponent = World.EntityManager.GetOrAddComponent<ShapeComponent>(bulletEntity);
        shapeComponent.Shapes.Add(new CircleShapeData(1f));
        
        var graphComponent = World.EntityManager.GetOrAddComponent<GraphComponent>(bulletEntity);
        graphComponent.Type = GraphType.Bullet;

        weaponComponent.WeaponShootCooldown = 0.1f;
    }
}