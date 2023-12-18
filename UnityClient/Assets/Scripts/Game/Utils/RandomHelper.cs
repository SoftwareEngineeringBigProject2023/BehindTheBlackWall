namespace Game.Utils
{
    public class RandomHelper
    {
        public static int RandomInt(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}