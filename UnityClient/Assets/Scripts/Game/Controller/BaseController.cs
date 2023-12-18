using SEServer.Data;
using SEServer.Data.Interface;
using UnityEngine;

namespace Game.Controller
{
    public abstract class BaseController
    {
        public bool IsDestroy { get; private set; } = false;
        public World World => MapperManager.World;
        public EntityManager EntityManager => MapperManager.World.EntityManager;
        
        public EntityMapperManager MapperManager { get; set; } = null!;

        private bool _isStart = false;
        
        public void Init()
        {
            OnInit();
        }
        
        protected virtual void OnInit()
        {
            
        }
        
        protected virtual void OnStart()
        {
            
        }

        public void Destroy()
        {
            IsDestroy = true;
            OnDestroy();
        }
        
        protected virtual void OnDestroy()
        {
            
        }

        public void Update()
        {
            if (!_isStart)
            {
                _isStart = true;
                OnStart();
            }
            
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
            
        }
    }
}