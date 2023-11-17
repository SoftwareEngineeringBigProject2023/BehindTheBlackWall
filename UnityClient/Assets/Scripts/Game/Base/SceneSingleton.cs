using Game.Framework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 场景中单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T>
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<T>();

                return _instance;
            }
        }
        
        public static T I => Instance;

        protected virtual void Awake()
        {
            if (_instance == null)
                _instance = this as T;
            else
                Destroy(gameObject);
        }
    }
}