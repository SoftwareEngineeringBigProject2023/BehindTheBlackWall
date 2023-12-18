using System;
using Game.Binding;
using Game.Framework;
using Game.Utils;
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
        private GraphType GraphType { get; set; }
        private int GraphRes { get; set; }

        protected override void OnInit()
        {
            var graphComponent = GraphComponent;
            GraphType = graphComponent.Type;
            GraphRes = graphComponent.Res;
            var prefab = GetPrefab();
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

        private GameObject GetPrefab()
        {
            switch (GraphType)
            {
                case GraphType.None:
                    break;
                case GraphType.Unit:
                    return GameManager.Res.LoadAsset<GameObject>("Assets/BuildRes/Prefab/Unit.prefab");
                case GraphType.Bullet:
                {
                    switch (GraphRes)
                    {
                        case 0:
                            return GameManager.Res.LoadAsset<GameObject>("Assets/BuildRes/Prefab/Bullet.prefab");
                        case 1:
                            return GameManager.Res.LoadAsset<GameObject>("Assets/BuildRes/Prefab/Bullet_1.prefab");
                        case 2:
                            return GameManager.Res.LoadAsset<GameObject>("Assets/BuildRes/Prefab/Bullet_2.prefab");
                        case 3:
                            return GameManager.Res.LoadAsset<GameObject>("Assets/BuildRes/Prefab/Bullet_3.prefab");
                        default:
                            return GameManager.Res.LoadAsset<GameObject>("Assets/BuildRes/Prefab/Bullet.prefab");
                    }
                }
                case GraphType.PickData:
                {
                    return  GameManager.Res.LoadAsset<GameObject>("Assets/BuildRes/Prefab/PickData.prefab");;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }

        protected override void OnDestroy()
        {
            if (GraphObject != null)
            {
                if (GraphType == GraphType.Bullet)
                {
                    var prefab = GetBulletHitPrefab();
                    if (prefab != null)
                    {
                        var eft = GameObject.Instantiate(prefab, GraphObject.transform.position, Quaternion.identity);
                        var binding = eft.AddComponent<GameEffectBinding>();
                        binding.LifeTime = 1f;
                    }
                }
                
                Object.Destroy(GraphObject);
            }
        }

        protected GameObject GetBulletHitPrefab()
        {
            switch (GraphRes)
            {
                case 1:
                    return GameManager.Res.LoadAsset<GameObject>("Assets/BuildRes/Prefab/Hit_1.prefab");
                case 2:
                    return GameManager.Res.LoadAsset<GameObject>("Assets/BuildRes/Prefab/Hit_2.prefab");
                case 3:
                    return GameManager.Res.LoadAsset<GameObject>("Assets/BuildRes/Prefab/Hit_3.prefab");
                default:
                    return GameManager.Res.LoadAsset<GameObject>("Assets/BuildRes/Prefab/Hit_1.prefab");
            }

            return null;
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