using System.Numerics;
using SEServer.Data;
using SEServer.GameData;

namespace SEServer.Game;

public class MoveSystem : ISystem
{
    public World World { get; set; }
    public void Init()
    {
        
    }

    public void Update()
    {
        var collection = World.EntityManager.GetComponentDataCollection<MoveInputComponent, TransformComponent>();
        foreach (var valueTuple in collection)
        {
            var (moveInput, position) = valueTuple;
            if(moveInput.Input.ToSystemVector2() == Vector2.Zero)
                continue;
            
            var positionValue = position.Position.ToSystemVector2();
            positionValue += moveInput.Input.ToSystemVector2() * World.Time.DeltaTime;
            position.Position = positionValue.ToSVector2();
            position.IsDirty = true;
            moveInput.Input = new SVector2(0, 0);
        }
    }
}