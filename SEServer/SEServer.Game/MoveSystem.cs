using SEServer.Data;
using SEServer.GameData;

namespace SEServer.Game;

public class MoveSystem : ISystem
{
    public World World { get; set; }
    public void Update()
    {
        var collection = World.EntityManager.GetComponentDataCollection<MoveInputComponent, TransformComponent>();
        foreach (var valueTuple in collection)
        {
            var (moveInput, position) = valueTuple;
            position.Position += moveInput.Input * World.Time.DeltaTime;
        }
    }
}