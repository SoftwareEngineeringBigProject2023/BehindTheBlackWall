using nkast.Aether.Physics2D.Dynamics;
using SEServer.Data;
using SEServer.Data.Interface;
using Phy2DWorld = nkast.Aether.Physics2D.Dynamics.World;

namespace SEServer.Game;

public class PhysicsSingletonComponent : IComponent
{
    public CId Id { get; set; }
    public EId EntityId { get; set; }
    public Phy2DWorld Phy2DWorld { get; set; } = null!;
    public SolverIterations iterations { get; set; }
    
    public Dictionary<EId, PhysicsData> PhysicsDataDic { get; set; } = new();
    public ContactListener ContactListener { get; set; }
}