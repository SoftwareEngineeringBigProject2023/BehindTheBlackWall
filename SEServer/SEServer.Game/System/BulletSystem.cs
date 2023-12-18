using nkast.Aether.Physics2D.Collision;
using nkast.Aether.Physics2D.Collision.Shapes;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.Game.Component;
using SEServer.GameData;
using SEServer.GameData.Builder;
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
        var bulletComponentCollection = World.EntityManager.GetComponentDataCollection<BulletComponent>();
        var physicsSingletonComponent = World.EntityManager.GetSingleton<PhysicsSingletonComponent>();
        
        foreach (var tuple in bulletComponentCollection)
        {
            var bulletComponent = tuple;
            if (bulletComponent.MarkDeleteDelayStep > 0)
            {
                bulletComponent.MarkDeleteDelayStep -= 1;
                if (bulletComponent.MarkDeleteDelayStep == 0)
                {
                    World.EntityManager.Entities.MarkAsToBeDelete(bulletComponent.EntityId);
                }
            }
            
            CheckBulletCollision(bulletComponent, physicsSingletonComponent);
            MoveBullet(bulletComponent);
        }
        
    }

    private void CheckBulletCollision(BulletComponent bullet, PhysicsSingletonComponent physicsSingleton)
    {
        var curTransform = World.EntityManager.GetComponent<TransformComponent>(bullet.EntityId);
        if (curTransform == null)
        {
            return;
        }
        
        if (bullet.Properties.TryGetValue(BulletType.IGNORE_WALL_STEPS, out var ignoreWallSteps) && ignoreWallSteps > 0)
        {
            bullet.Properties[BulletType.IGNORE_WALL_STEPS] -= 1;
        }
        
        //var worldDelta = World.Time.DeltaTime;
        var direction = SVector2.FromAngle(curTransform.Rotation);
        var startPoint = curTransform.Position - direction.normalized * bullet.Speed * World.Time.DeltaTime * 0.5f;
        var endPoint = curTransform.Position + direction.normalized * bullet.Speed * World.Time.DeltaTime * 0.5f;

        bool found = false;
        SVector2 findNormal = default;
        SVector2 findPoint = default;
        PhysicsData? findPhyData = null;
        RigidbodyComponent? findRigidbody = null;
        physicsSingleton.Phy2DWorld.RayCast((fixture, point, normal, fraction) =>
        {
            if(fixture.Body.Tag is PhysicsData physicsData)
            {
                findRigidbody = World.EntityManager.GetComponent<RigidbodyComponent>(physicsData.BindEntityId);
                if(findRigidbody == null)
                    return -1f;
                
                var tag = findRigidbody.Tag;
                if (tag != PhysicsTag.WALL && tag != PhysicsTag.UNIT)
                    return -1f;

                found = true;
                findPhyData = physicsData;
                findNormal = normal.ToSVector2();
                findPoint = point.ToSVector2();
                return 0f;
            }
        
            return -1f;
        }, startPoint.ToPhysicsVector2(), endPoint.ToPhysicsVector2());

        if (found)
        {
            switch (findRigidbody!.Tag)
            {
                case PhysicsTag.UNIT:
                    OnBulletHitUnit(bullet, physicsSingleton, findRigidbody, findNormal, findPoint);
                    break;
                case PhysicsTag.WALL:
                    if(ignoreWallSteps <= 0)
                    {
                        OnBulletHitWall(bullet, physicsSingleton, findRigidbody, findNormal, findPoint);
                    }
                    break;
            }
        }
    }
    
    private void OnBulletHitUnit(BulletComponent bullet, PhysicsSingletonComponent physicsSingleton,
        RigidbodyComponent targetRigidbody, SVector2 hitNormal, SVector2 hitPoint)
    {
        var targetProperty = World.EntityManager.GetComponent<PropertyComponent>(targetRigidbody.EntityId);
        if (targetProperty == null)
            return;
        
        if (bullet.CreatorId == targetProperty.EntityId.Id)
            return;
        
        var targetPlayerComponent = World.EntityManager.GetComponent<PlayerComponent>(targetProperty.EntityId);
        if (targetPlayerComponent == null)
            return;
        
        DestroyBullet(bullet, hitPoint);
        
        targetProperty.Hp -= bullet.Damage;
        
        var playerGlobalNotify = World.EntityManager.GetSingleton<PlayerNotifyGlobalComponent>();
        playerGlobalNotify.AddNotifyMessage(new UnitInjuryNotifyGlobalMessageBuilder()
            .SetEntityId(targetProperty.EntityId.Id)
            .SetDamage((int)bullet.Damage)
            .Build());
        
        if (targetProperty.Hp <= 0)
        {
            
            playerGlobalNotify.AddNotifyMessage(new PlayerDefeatNotifyGlobalMessageBuilder()
                .SetPlayerId(targetPlayerComponent.EntityId.Id)
                .SetDefeatedId(targetRigidbody.EntityId.Id)
                .Build());
            
            var scoreComponent = World.EntityManager.GetComponent<ScoreViewComponent>(new EId()
            {
                Id = bullet.CreatorId
            });
            if (scoreComponent != null)
            {
                var score = 0;
                score += 100;
                var otherScoreComponent = World.EntityManager.GetComponent<ScoreViewComponent>(targetProperty.EntityId);
                if (otherScoreComponent != null)
                {
                    score += (int)(otherScoreComponent.Score * 0.5f);
                }

                scoreComponent.Score += score;

                if (score > 0)
                {
                    var globalNotify = World.EntityManager.GetSingleton<PlayerNotifyGlobalComponent>();
                    globalNotify.AddNotifyMessage(new UnitScoreChangedNotifyGlobalMessageBuilder()
                        .SetEntityId(scoreComponent.EntityId.Id)
                        .SetScore(score)
                        .Build());
                }

                
                World.EntityManager.MarkAsDirty(scoreComponent);
            }
        }
        // else
        // {
        //     var scoreComponent = World.EntityManager.GetComponent<ScoreViewComponent>(playerComponent.EntityId);
        //     if (scoreComponent != null)
        //     {
        //         scoreComponent.Score += (int)bullet.Damage;
        //         World.EntityManager.MarkAsDirty(scoreComponent);
        //     }
        // }

        targetProperty.HeartTimer = World.Time.TotalTime;
        
        World.EntityManager.MarkAsChanged(targetProperty);
        
        var curTransform = World.EntityManager.GetComponent<TransformComponent>(bullet.EntityId);
        if (curTransform == null)
        {
            return;
        }

        var direction = SVector2.FromAngle(curTransform.Rotation);
        physicsSingleton.AddForce(targetRigidbody.EntityId, direction.normalized * 200f);
    }
    
    private void OnBulletHitWall(BulletComponent bullet, PhysicsSingletonComponent physicsSingleton,
        RigidbodyComponent targetRigidbody, SVector2 hitNormal, SVector2 hitPoint)
    {
        if(bullet.Properties.TryGetValue(BulletType.BOUNCE, out var bounce) && bounce > 0)
        {
            BounceBullet(bullet, physicsSingleton, targetRigidbody, hitNormal, hitPoint);
            bullet.Properties[BulletType.BOUNCE] -= 1;
            bullet.Properties[BulletType.IGNORE_WALL_STEPS] = 2;
        }
        else
        {
            DestroyBullet(bullet, hitPoint);
        }
    }

    private void BounceBullet(BulletComponent bullet, PhysicsSingletonComponent physicsSingleton, 
        RigidbodyComponent targetRigidbody, SVector2 hitNormal, SVector2 hitPoint)
    {
        var curTransform = World.EntityManager.GetComponent<TransformComponent>(bullet.EntityId);
        if (curTransform == null)
        {
            return;
        }

        var bulletLineMoveComponent = World.EntityManager.GetComponent<BulletLineMoveComponent>(bullet.EntityId);
        if (bulletLineMoveComponent == null)
        {
            return;
        }
        
        var direction = bulletLineMoveComponent.Direction;
        var reflectDirection = SVector2.Reflect(direction, hitNormal);
        bulletLineMoveComponent.Direction = reflectDirection;
        curTransform.Position = hitPoint;
        curTransform.Rotation = reflectDirection.Angle();

        bullet.SkipMove = 1;
    }

    private void MoveBullet(BulletComponent bulletComponent)
    {
        if(bulletComponent.SkipMove > 0)
        {
            bulletComponent.SkipMove -= 1;
            return;
        }
        
        var bulletLineMoveComponent = World.EntityManager.GetComponent<BulletLineMoveComponent>(bulletComponent.EntityId);
        if (bulletLineMoveComponent != null)
        {
            MoveBulletLine(bulletComponent, bulletLineMoveComponent);
            return;
        }
    }

    private void MoveBulletLine(BulletComponent bulletComponent, BulletLineMoveComponent bulletLineMoveComponent)
    {
        var moveDistance = bulletLineMoveComponent.Speed * World.Time.DeltaTime;
        var moveVector = bulletLineMoveComponent.Direction * moveDistance;
        bulletLineMoveComponent.Distance += moveDistance;
        
        var transformComponent = World.EntityManager.GetComponent<TransformComponent>(bulletComponent.EntityId);
        if (transformComponent == null)
        {
            return;
        }
        
        transformComponent.Position += moveVector;
        World.EntityManager.MarkAsDirty(transformComponent);
        
        if (bulletLineMoveComponent.Distance > bulletLineMoveComponent.MaxDistance)
        {
            World.EntityManager.Entities.MarkAsToBeDelete(bulletComponent.EntityId);
            return;
        }
    }
    
    private void DestroyBullet(BulletComponent bullet, SVector2 hitPoint)
    {
        // rigidbody.SetVelocity = SVector2.Zero;
        // rigidbody.SetPosition = hitPoint;
        
        bullet.MarkDeleteDelayStep = 1;
    }
}