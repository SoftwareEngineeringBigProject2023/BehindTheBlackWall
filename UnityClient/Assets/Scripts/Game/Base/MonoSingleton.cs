using UnityEngine;

namespace Game
{
    /// <summary>
    /// 全局单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    if (!Application.isPlaying)
                    {
                        Debug.LogError($"不能在非播放模式下创建单例：{typeof(T).Name}");
                        return null;
                    }
                    
                    var obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }

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
            
            DontDestroyOnLoad(gameObject);
        }
    }
}