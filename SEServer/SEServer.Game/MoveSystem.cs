using System.Numerics;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData;

namespace SEServer.Game;

[Priority(20)]
public class MoveSystem : ISystem
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
            if(moveInput.Input == SVector2.Zero && rigidbody.Velocity == SVector2.Zero)
                continue;
            
            var speed = property.Speed;
            var velocity = moveInput.Input * speed;
            rigidbody.SetVelocity = velocity;
            World.EntityManager.MarkAsChanged(rigidbody);
            
            property.LineVelocity = velocity;
            World.EntityManager.MarkAsChanged(property);

            moveInput.Input = new SVector2(0, 0);
        }
    }
}