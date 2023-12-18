using System;
using Game.Utils;
using SEServer.Data.Interface;
using SEServer.GameData;
using SEServer.GameData.Component;
using Sirenix.OdinInspector;
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
        public GraphComponent GraphComponent => GetEComponent<GraphComponent>();

        protected override void OnUpdate()
        {
            var graphController = GraphController.Value;
            if(graphController == null)
                return;

            var transformComponent = TransformComponent;
            var transform = graphController.GraphObject.transform;

            var realPosition = transformComponent.Position.ToUVector2();
            
            var graphComponent = GraphComponent;

            if (graphComponent != null)
            {
                switch (graphComponent.Type)
                {
                    case GraphType.Bullet:
                        if (Vector3.Distance(transform.position, realPosition) < 10f)
                        {
                            // 如果变化小，则缓动过去
                            transform.position = Vector3.Lerp(transform.position, realPosition, Time.deltaTime * 30);
                        }
                        else
                        {
                            // 如果变化大，则直接过去
                            transform.position = realPosition;
                        }
                        break;
                    case GraphType.None:
                    case GraphType.Unit:
                    default:
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
                        break;
                }
            }
            
            
            
            var realRotation = transformComponent.Rotation;
            transform.eulerAngles = new Vector3(0, 0, realRotation);
            // var realRotation = transformComponent.Rotation;
            // if (Math.Abs(transform.eulerAngles.z - realRotation) < 10f)
            // {
            //     // 如果变化小，则缓动过去
            //     transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, realRotation), Time.deltaTime * 10);
            // }
            // else
            // {
            //     // 如果变化大，则直接过去
            //     transform.eulerAngles = new Vector3(0, 0, realRotation);
            // }
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