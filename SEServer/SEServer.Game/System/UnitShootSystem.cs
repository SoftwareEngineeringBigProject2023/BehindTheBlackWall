using nkast.Aether.Physics2D.Collision.Shapes;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData;
using SEServer.GameData.Builder;
using SEServer.GameData.Component;
using SEServer.GameData.CTData;
using SEServer.GameData.Data;

namespace SEServer.Game.System;

/// <summary>
/// 射击系统
/// </summary>
[Priority(30)]
public class UnitShootSystem : ISystem
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

            var weaponCTData = weaponComponent.WeaponCTData;
            if (weaponCTData == null)
            {
                continue;
            }
            
            if(weaponComponent.WeaponShootCooldown > 0)
                weaponComponent.WeaponShootCooldown -= World.Time.DeltaTime;
            
            if (shootInputComponent.TriggerShoot && weaponComponent.WeaponShootCooldown <= 0)
            {
                ShootFire(weaponComponent, weaponCTData, propertyComponent);
            }
            
            shootInputComponent.TriggerShoot = false;
        }
    }

    private void ShootFire(WeaponComponent weaponComponent, WeaponCTData weaponCtData,
        PropertyComponent propertyComponent)
    {
        foreach (var bulletSubData in weaponCtData.ShootBullets)
        {
            var bulletCtData = World.ServerContainer.Get<IConfigTable>().Get<BulletCTData>(bulletSubData.BulletId);
            if (bulletCtData == null)
            {
                continue;
            }
            
            var offsetPos = bulletSubData.Position;
            var rotation = bulletSubData.Angle;
            var range = weaponCtData.Range;
            var speed = weaponCtData.BulletSpeed;
            var damage = weaponCtData.BaseDamage;
            var properties = bulletCtData.Properties;
            var resId = bulletCtData.GraphId;
            
            CreateBullet(weaponComponent, propertyComponent, offsetPos, rotation, range, speed, damage, resId, properties);
        }
        
        weaponComponent.WeaponShootCooldown = weaponCtData.ShootInterval;

        var weaponId = World.ServerContainer.Get<IConfigTable>().GetIndexByData(weaponCtData);
        
        var playerNotifyGlobal = World.EntityManager.GetSingleton<PlayerNotifyGlobalComponent>();
        playerNotifyGlobal.AddNotifyMessage(new UnitShootNotifyGlobalMessageBuilder()
            .SetEntityId(propertyComponent.EntityId.Id)
            .SetWeaponId(weaponId)
            .Build());
    }

    private void CreateBullet(WeaponComponent weaponComponent, PropertyComponent propertyComponent,
        SVector2 offsetPos, float rotation, float range, float speed, float damage, int resId,
        Dictionary<string, int>? properties)
    {
        var ownerTransform = World.EntityManager.GetComponent<TransformComponent>(weaponComponent.EntityId);
        var initPos = SVector2.Zero;
        if(ownerTransform != null)
            initPos = ownerTransform.Position;
        
        var bulletEntity = World.EntityManager.CreateEntity();
        var bulletComponent = World.EntityManager.GetOrAddComponent<BulletComponent>(bulletEntity);
        bulletComponent.Damage = damage;
        bulletComponent.Speed = speed;
        bulletComponent.CreatorId = propertyComponent.EntityId.Id;
        if(properties?.TryGetValue(BulletType.BOUNCE, out var bounce) == true)
        {
            bulletComponent.Properties[BulletType.BOUNCE] = bounce;
        }
        
        var bulletLineMoveComponent = World.EntityManager.GetOrAddComponent<BulletLineMoveComponent>(bulletEntity);
        bulletLineMoveComponent.Speed = speed;

        var bulletInitPos = new SVector2(1f, 0f);
        var direction = bulletInitPos.Rotate(propertyComponent.TargetAimRotation + rotation);
        bulletLineMoveComponent.Direction = direction.normalized;
        bulletLineMoveComponent.MaxDistance = range;
        
        var transformComponent = World.EntityManager.GetOrAddComponent<TransformComponent>(bulletEntity);
        transformComponent.Position = initPos + offsetPos;
        transformComponent.Rotation = propertyComponent.TargetAimRotation + rotation;

        // var rigidbodyComponent = World.EntityManager.GetOrAddComponent<RigidbodyComponent>(bulletEntity);
        // rigidbodyComponent.BodyType = PhysicsBodyType.Dynamic;
        // rigidbodyComponent.IsBullet = true;
        // rigidbodyComponent.IsTrigger = true;
        // rigidbodyComponent.SetRotation = propertyComponent.TargetAimRotation + rotation;
        //
        // var shapeComponent = World.EntityManager.GetOrAddComponent<ShapeComponent>(bulletEntity);
        // shapeComponent.Shapes.Add(new RectangleShapeData(0.1f, 0.2f));
        
        var graphComponent = World.EntityManager.GetOrAddComponent<GraphComponent>(bulletEntity);
        graphComponent.Type = GraphType.Bullet;
        graphComponent.Res = resId;
    }
}