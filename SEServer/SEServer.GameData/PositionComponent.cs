using System.Numerics;
using SEServer.Data;

namespace SEServer.GameData;

public class PositionComponent : IS2CComponent
{
    public CId Id { get; set; }
    public EId EntityId { get; set; }
    public bool IsDirty { get; set; }
    
    public Vector2 position;
}