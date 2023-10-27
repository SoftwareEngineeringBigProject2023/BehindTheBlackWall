using SEServer.Data;
using UnityEngine;

namespace Game
{
    public static class TransformExtension
    {
        public static Vector2 ToUnityVector2(this System.Numerics.Vector2 vector2)
        {
            return new Vector2(vector2.X, vector2.Y);
        }
        
        public static Vector3 ToUnityVector3(this System.Numerics.Vector3 vector3)
        {
            return new Vector3(vector3.X, vector3.Y, vector3.Z);
        }
        
        public static System.Numerics.Vector2 ToSystemVector2(this Vector2 vector2)
        {
            return new System.Numerics.Vector2(vector2.x, vector2.y);
        }
        
        public static System.Numerics.Vector3 ToSystemVector3(this Vector3 vector3)
        {
            return new System.Numerics.Vector3(vector3.x, vector3.y, vector3.z);
        }
        
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