using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Framework
{
    public abstract class UIBase
    {
        public const string UI_BIND_PREFIX = "b:";
        
        public UIBase(string prefabPath, UILayer uiLayer)
        {
            var prefab = ResManager.Instance.LoadAsset<GameObject>(prefabPath);
            BindGameObject = Object.Instantiate(prefab);
            BindTransform = BindGameObject.transform;
            Layer = uiLayer;
        }

        private bool _isShow;
        private bool _isInit;
        private Dictionary<string, GameObject> _bindGameObjects = new Dictionary<string, GameObject>();

        /// <summary>
        /// UI绑定的GameObject
        /// </summary>
        public GameObject BindGameObject { get; private set; }
        /// <summary>
        /// UI绑定的Transform
        /// </summary>
        public Transform BindTransform { get; private set; }
        /// <summary>
        /// UI的层级
        /// </summary>
        public UILayer Layer { get; private set; }

        /// <summary>
        /// 显示UI
        /// </summary>
        public void Show()
        {
            if (_isShow)
            {
                return;
            }
            
            _isShow = true;
            if (!_isInit)
            {
                _isInit = true;
                Init();
            }
            
            DoShowAnimation();
        }
        
        /// <summary>
        /// 隐藏UI
        /// </summary>
        public void Hide()
        {
            DoHideAnimation();
        }
        
        public void HideImmediately()
        {
            OnHide();
            DestroyUI();
        }
        
        public T Get<T>(string key) where T : UnityEngine.Component
        {
            if (_bindGameObjects.TryGetValue(key, out var gameObject))
            {
                return gameObject.GetComponent<T>();
            }

            return null;
        }

        private void DestroyUI()
        {
            if (BindGameObject == null)
            {
                return;
            }
            
            UIManager.Instance.UnregisterUI(this);
            Object.Destroy(BindGameObject);
        }

        private void Init()
        {
            UIManager.Instance.RegisterUI(this);

            CollectBindComponent();
            OnInit();
        }

        private void CollectBindComponent()
        {
            // 遍历所有子节点，查找绑定的GameObject
            var queue = new Queue<Transform>();
            queue.Enqueue(BindTransform);
            while (queue.Count > 0)
            {
                var transform = queue.Dequeue();
                var childCount = transform.childCount;
                for (var i = 0; i < childCount; i++)
                {
                    var child = transform.GetChild(i);
                    queue.Enqueue(child);
                }

                var name = transform.name;
                if (name.StartsWith(UI_BIND_PREFIX))
                {
                    var key = name.Substring(UI_BIND_PREFIX.Length);
                    _bindGameObjects.Add(key, transform.gameObject);
                }
            }
        }

        protected virtual void OnInit()
        {
            
        }

        /// <summary>
        /// 显示完毕后调用<code>OnShown()</code>
        /// </summary>
        protected virtual void DoShowAnimation()
        {
            OnShown();
        }

        protected virtual void OnShown()
        {
            
        }
        
        /// <summary>
        /// 消失完毕后调用<code>HideImmediately()</code>
        /// </summary>
        protected virtual void DoHideAnimation()
        {
            HideImmediately();
        }

        protected virtual void OnHide()
        {
            
        }
    }
}