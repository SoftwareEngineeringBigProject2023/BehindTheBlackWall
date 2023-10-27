using System.Numerics;
using SEServer.Data;

namespace SEServer.GameData;

public class MoveInputComponent : IC2SComponent
{
    public CId Id { get; set; }
    public EId EntityId { get; set; }
    public bool IsDirty { get; set; }
    public PlayerId Owner { get; set; }
    
    public Vector2 input;
}