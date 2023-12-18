using SEServer.Data;
using UnityEngine;

namespace Game.Utils
{
    public static class TransformExtension
    {
        public static Vector2 ToUVector2(this SVector2 sVector2)
        {
            return new Vector2(sVector2.X, sVector2.Y);
        }
        
        public static SVector2 ToSVector2(this Vector2 vector2)
        {
            return new SVector2(vector2.x, vector2.y);
        }
    }
}