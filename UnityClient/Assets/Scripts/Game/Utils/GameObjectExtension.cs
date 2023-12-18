namespace Game.Utils
{
    public static class GameObjectExtension
    {
        public static T GetOrAddComponent<T>(this UnityEngine.GameObject gameObject) where T : UnityEngine.Component
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }
        
        public static T GetOrAddComponent<T>(this UnityEngine.Component component) where T : UnityEngine.Component
        {
            return component.gameObject.GetOrAddComponent<T>();
        }
    }
}