using SEServer.Data;
using SEServer.GameData;

namespace SEServer.Game;

public class MoveSystem : ISystem
{
    public World World { get; set; }
    public void Update()
    {
        var collection = World.GetComponentDataCollection<MoveInputComponent, PositionComponent>();
        foreach (var valueTuple in collection)
        {
            var (moveInput, position) = valueTuple;
            position.position += moveInput.input * World.Time.DeltaTime;
        }
    }
}