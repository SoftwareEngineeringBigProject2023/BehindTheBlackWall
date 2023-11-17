using System;
using Game.Binding;
using SEServer.Data.Interface;
using SEServer.GameData;
using SEServer.GameData.Component;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Controller
{
    public class GraphController : BaseComponentController
    {
        public GraphComponent GraphComponent => GetEComponent<GraphComponent>();
        public TransformComponent TransformComponent => GetEComponent<TransformComponent>();
        public GameObject GraphObject { get; set; }

        protected override void OnInit()
        {
            var graphComponent = GraphComponent;
            var prefab = GetPrefab(graphComponent);
            var initPos = Vector3.zero;
            
            var transform = TransformComponent;
            if (transform != null)
            {
                initPos = transform.Position.ToUVector2();
            }
            
            GraphObject = Object.Instantiate(prefab, MapperManager.EntityRoot);
            GraphObject.name = $"graph_{ControllerMapper.EntityMapper.EntityId}";
            GraphObject.transform.localPosition = initPos;
        }

        private GameObject GetPrefab(GraphComponent graphComponent)
        {
            switch (graphComponent.Type)
            {
                case GraphType.None:
                    break;
                case GraphType.Unit:
                    return Resources.Load<GameObject>("Prefab/Unit");
                case GraphType.Bullet:
                    return Resources.Load<GameObject>("Prefab/Bullet");
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }

        protected override void OnDestroy()
        {
            if (GraphObject != null)
            {
                Object.Destroy(GraphObject);
            }
        }
    }
    
    public class GraphControllerBuilder : BaseControllerBuilder
    {
        public override Type BindType { get; } = typeof(GraphComponent);
        
        public override BaseComponentController BuildController()
        {
            return new GraphController();
        }
    }
}