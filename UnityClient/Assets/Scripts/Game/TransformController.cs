using System;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData;
using UnityEngine;

namespace Game
{
    public class TransformController : BaseController
    {
        public TransformComponent TransformComponent => GetEComponent<TransformComponent>();
        public PropertyComponent PropertyComponent => GetEComponent<PropertyComponent>();
        
        private void Update()
        {
            var position = TransformComponent.Position.ToUVector2();
            if (Vector3.Distance(transform.position, position) < 5f)
            {
                // 如果变化小，则缓动过去
                transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 10);
                // if (PropertyComponent != null)
                // {
                //     // 预测未来移动
                //     var velocity = PropertyComponent.LineVelocity.ToUVector2();
                //     var futurePosition = position + velocity * Time.deltaTime;
                //     transform.position = Vector3.Lerp(transform.position, futurePosition, Time.deltaTime * 10);
                // }
                // else
                // {
                //     
                // }
            }
            else
            {
                // 如果变化大，则直接过去
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