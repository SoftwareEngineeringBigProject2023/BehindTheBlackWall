using SEServer.Data.Interface;
using UnityEngine;

namespace Game.Controller
{
    public abstract class BaseController
    {
        public bool IsDestroy { get; private set; } = false;
        
        public EntityMapperManager MapperManager { get; set; } = null!;
        

        public void Init()
        {
            OnInit();
        }
        
        protected virtual void OnInit()
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
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
            
        }
    }
}