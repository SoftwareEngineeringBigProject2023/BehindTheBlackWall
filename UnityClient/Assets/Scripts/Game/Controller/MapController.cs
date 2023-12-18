using System;
using Cinemachine;
using Game.Binding;
using Game.Framework;
using Game.MapEditor;
using Game.Utils;
using SEServer.Data.Interface;
using SEServer.GameData.Component;
using SEServer.GameData.CTData;
using SEServer.GameData.Data;
using UnityEngine;

namespace Game.Controller
{
    public class MapController : BaseComponentController
    {
        public MapInfoGlobalComponent MapComponent => GetEComponent<MapInfoGlobalComponent>();
        public MapEditorBehaviour Map { get; set; }
        public MapEditorLoader MapLoader { get; set; }
        
        protected override void OnInit()
        {
            base.OnInit();
            
            var go = GameManager.Res.Spawn("Assets/BuildRes/Prefab/GameMap.prefab", MapperManager.EntityRoot);
            go.transform.position = Vector3.zero;
            Map = go.GetComponent<MapEditorBehaviour>();
            MapLoader = go.GetComponent<MapEditorLoader>();

            var mapData = ClientBehaviour.I.ClientInstance.ServerContainer.Get<IConfigTable>()
                .Get<GameMapCTData>(MapComponent.MapId);
            MapLoader.LoadMap(mapData);
            
            BindCameraBound(mapData);
        }

        private void BindCameraBound(GameMapCTData mapData)
        {
            var virtualCamera = CameraStaticBinding.GetVirtualCamera();
            if(virtualCamera == null)
                return;
            
            var polygonCollider = MapperManager.EntityRoot.GetOrAddComponent<PolygonCollider2D>();
            
            var rect = mapData.MapSize;
            var edge = 0.5f;
            var leftBottom = new Vector2(-rect.X / 2f + edge, -rect.Y / 2f + edge);
            var rightBottom = new Vector2(rect.X / 2f - edge, -rect.Y / 2f + edge);
            var rightTop = new Vector2(rect.X / 2f - edge, rect.Y / 2f - edge);
            var leftTop = new Vector2(-rect.X / 2f + edge, rect.Y / 2f - edge);
            
            polygonCollider.SetPath(0, new []
            {
                rightTop,
                rightBottom,
                leftBottom,
                leftTop,
                rightTop,
            });
            
            var cineMachineConfiner = virtualCamera.GetOrAddComponent<CinemachineConfiner>();
            cineMachineConfiner.m_BoundingShape2D = polygonCollider;
            cineMachineConfiner.m_ConfineMode = CinemachineConfiner.Mode.Confine2D;
        }
    }
    
    public class MapControllerBuilder : BaseControllerBuilder
    {
        public override Type BindType { get; } = typeof(MapInfoGlobalComponent);
        
        public override BaseComponentController BuildController()
        {
            return new MapController();
        }
    }
}