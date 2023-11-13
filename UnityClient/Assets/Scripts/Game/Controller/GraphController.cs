using System;
using SEServer.Data.Interface;
using SEServer.GameData;
using UnityEngine;

namespace Game.Controller
{
    public class GraphController : BaseController
    {
        public GraphComponent GraphComponent => GetEComponent<GraphComponent>();

        private void Awake()
        {
            var prefab = Resources.Load<GameObject>("Prefab/Unit");
            var go = Instantiate(prefab, gameObject.transform);
            go.transform.localPosition = Vector3.zero;
        }

        private void Update()
        {
            
        }
    }
    
    public class GraphControllerBuilder : BaseControllerBuilder
    {
        public override Type BindType { get; } = typeof(GraphComponent);
        
        public override BaseController BuildController(GameObject gameObject, IComponent component)
        {
            return gameObject.AddComponent<GraphController>();
        }
    }
}