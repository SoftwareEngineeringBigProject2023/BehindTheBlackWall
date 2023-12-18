

using UnityEngine;

namespace Game.Utils
{
    public static class ColorExtension
    {
        public static float[] ToFloatArray(this Color color)
        {
            return new float[] { color.r, color.g, color.b, color.a};
        }
        
        public static Color ToColor(this float[] color)
        {
            if(color == null || color.Length == 0)
                return Color.white;
        
            if (color.Length >= 4)
                return new Color(color[0], color[1], color[2], color[3]);
            
            if (color.Length == 3)
                return new Color(color[0], color[1], color[2]);
            
            return Color.white;
        }
        
    }
}