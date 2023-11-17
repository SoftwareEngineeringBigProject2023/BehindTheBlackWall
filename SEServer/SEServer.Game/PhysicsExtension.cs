using System.Numerics;
using SEServer.Data;

namespace SEServer.Game;

public static class PhysicsExtension
{
    public static SVector2 ToSVector2(this nkast.Aether.Physics2D.Common.Vector2 vector2)
    {
        return new SVector2(vector2.X, vector2.Y);
    }
    
    public static nkast.Aether.Physics2D.Common.Vector2 ToPhysicsVector2(this SVector2 sVector2)
    {
        return new nkast.Aether.Physics2D.Common.Vector2(sVector2.X, sVector2.Y);
    }
    
    public static PhysicsData? GetPhysicsData(this PhysicsSingletonComponent physicsSingletonComponent, EId entityId)
    {
        if (physicsSingletonComponent.PhysicsDataDic.TryGetValue(entityId, out var physicsData))
        {
            return physicsData;
        }
        
        return null;
    }
}