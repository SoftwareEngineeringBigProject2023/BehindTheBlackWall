using System;
using SEServer.Data.Interface;
using SEServer.GameData;
using SEServer.GameData.Component;
using UnityEngine;

namespace Game.Controller
{
    public class TransformController : BaseComponentController
    {
        public TransformController()
        {
            GraphController = new LazyControllerGetter<GraphController>(this);
        }
        
        public LazyControllerGetter<GraphController> GraphController { get; }
        public TransformComponent TransformComponent => GetEComponent<TransformComponent>();
        
        protected override void OnUpdate()
        {
            var graphController = GraphController.Value;
            if(graphController == null)
                return;

            var transformComponent = TransformComponent;
            var transform = graphController.GraphObject.transform;
            
            var realPosition = transformComponent.Position.ToUVector2();
            
            if (Vector3.Distance(transform.position, realPosition) < 10f)
            {
                // 如果变化小，则缓动过去
                transform.position = Vector3.Lerp(transform.position, realPosition, Time.deltaTime * 10);
            }
            else
            {
                // 如果变化大，则直接过去
                transform.position = realPosition;
            }
            
            var realRotation = transformComponent.Rotation;
            if (Math.Abs(transform.eulerAngles.z - realRotation) < 10f)
            {
                // 如果变化小，则缓动过去
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, realRotation), Time.deltaTime * 10);
            }
            else
            {
                // 如果变化大，则直接过去
                transform.eulerAngles = new Vector3(0, 0, realRotation);
            }
        }
    }
    
    public class TransformControllerBuilder : BaseControllerBuilder
    {
        public override Type BindType { get; } = typeof(TransformComponent);
        
        public override BaseComponentController BuildController()
        {
            return new TransformController();
        }
    }
}