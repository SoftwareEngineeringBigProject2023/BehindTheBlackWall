using System;
using SEServer.Data;
using SEServer.GameData;
using UnityEngine;

namespace Game
{
    public class TransformController : BaseController
    {
        public TransformComponent TransformComponent => GetEComponent<TransformComponent>();
        
        private void Update()
        {
            // 如果变化小，则缓动过去
            var position = TransformComponent.Position.ToUVector2();
            if (Vector3.Distance(transform.position, position) < 0.5f)
            {
                transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 10);
            }
            else
            {
                transform.position = position;
            }
        }
    }
    
    public class TransformControllerBuilder : BaseControllerBuilder
    {
        public override Type BindType { get; } = typeof(TransformComponent);
        
        public override BaseController BuildController(GameObject gameObject, IComponent component)
        {
            var controller = gameObject.AddComponent<TransformController>();
            return controller;
        }
    }
}