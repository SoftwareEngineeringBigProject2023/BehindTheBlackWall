using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData.Component;

namespace SEServer.Game.System;

[Priority(30)]
public class UnitMoveSystem : ISystem
{
    public World World { get; set; }
    public void Init()
    {
        
    }

    public void Update()
    {
        var collection = World.EntityManager.GetComponentDataCollection<MoveInputComponent, PropertyComponent, RigidbodyComponent>();
        foreach (var valueTuple in collection)
        {
            var (moveInput, property , rigidbody) = valueTuple;
            
            // 移动
            if (moveInput.Input != SVector2.Zero || rigidbody.Velocity != SVector2.Zero)
            {
                var speed = property.Speed;
                var velocity = moveInput.Input * speed;
                rigidbody.SetVelocity = velocity;
                World.EntityManager.MarkAsChanged(rigidbody);
            }

            // 转向瞄准
            if (Math.Abs(moveInput.TargetRotation - property.TargetAimRotation) > 0.01f)
            {
                var rotation = moveInput.TargetRotation;
                property.TargetAimRotation = rotation;
                World.EntityManager.MarkAsDirty(property);
            }
        }
    }
}