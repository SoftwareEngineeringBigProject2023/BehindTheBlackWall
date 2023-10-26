using System.Numerics;

namespace SEServer.Data;

public static class SStructExtension
{
    public static Vector2 ToSystemVector2(this SVector2 sVector2)
    {
        return new(sVector2.X, sVector2.Y);
    }
    
    public static SVector2 ToSVector2(this Vector2 vector2)
    {
        return new SVector2(vector2.X, vector2.Y);
    }
}