using System.Collections.Generic;
using Game.Utils;
using SEServer.Data;
using SEServer.GameData.CTData;
using SEServer.GameData.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.MapEditor
{
    public class MapEditorExporter : MonoBehaviour
    {
        private MapEditorBehaviour _mapEditorBehaviour;
        public MapEditorBehaviour MapEditor
        {
            get
            {
                if(_mapEditorBehaviour == null)
                    _mapEditorBehaviour = GetComponent<MapEditorBehaviour>();
                
                return _mapEditorBehaviour;
            }
        }

        public GameMapCTData ExportMap()
        {
            var mapData = new GameMapCTData();
            
            mapData.Backgrounds = ExportImage(MapEditor.BackgroundRoot);
            mapData.Decorations = ExportImage(MapEditor.DecorationRoot);
            mapData.Colliders = ExportCollider(MapEditor.DecorationRoot);
            mapData.SpecialPoints = ExportSpecialPoint(MapEditor.SpecialPointRoot);
            mapData.MapSize = MapEditor.MapEdgeSize.ToSVector2();
            mapData.PrimaryKey = MapEditor.MapId;
            mapData.DisplayName = MapEditor.MapName;

            return mapData;
        }

        private List<SpecialPointData> ExportSpecialPoint(Transform root)
        {
            var specialPointList = new List<SpecialPointData>();
            
            foreach (var specialPoint in root.GetComponentsInChildren<MapSpecialPoint>())
            {
                var specialPointData = new SpecialPointData();
                var specialPointTransform = specialPoint.transform;
                var pos = specialPointTransform.position;
                specialPointData.Id = specialPoint.pointId;
                specialPointData.Position = new SVector2(pos.x, pos.y);
                specialPointData.Type = specialPoint.pointType;

                specialPointList.Add(specialPointData);
            }
            
            return specialPointList;
        }

        private List<ImageData> ExportImage(Transform root)
        {
            var imageList = new List<ImageData>();
            
            foreach (var imageRenderer in root.GetComponentsInChildren<MapImageRenderer>())
            {
                var imageData = new ImageData();
                var spriteTransform = imageRenderer.transform;
                var pos = spriteTransform.position;
                var size = (Vector2)spriteTransform.lossyScale;
                var spriteRenderer = imageRenderer.GetComponent<SpriteRenderer>();
                imageData.Position = new SVector2(pos.x, pos.y);
                imageData.ZIndex = (int)(pos.z * 100f);
                imageData.Rotation = spriteTransform.rotation.eulerAngles.z;
                imageData.Size = size.ToSVector2();
                imageData.Path = imageRenderer.imagePath;
                imageData.Color = spriteRenderer.color.ToFloatArray();
                
                imageList.Add(imageData);
            }
            
            return imageList;
        }
        
        private List<ColliderDataBase> ExportCollider(Transform root)
        {
            var colliderList = new List<ColliderDataBase>();
            
            foreach (var mapCollider in root.GetComponentsInChildren<MapColliderBase>())
            {
                ColliderDataBase colliderData;
                var colliderTransform = mapCollider.transform;
                var pos = colliderTransform.position;
                
                switch (mapCollider)
                {
                    case MapColliderRect mapColliderRect:
                        var size = mapColliderRect.size;
                        var rectData = new ColliderDataRect();
                        rectData.Size = size.ToSVector2();
                        colliderData = rectData;
                        break;
                    case MapColliderCircle mapColliderCircle:
                        var circleData = new ColliderDataCircle();
                        circleData.Radius = mapColliderCircle.radius;
                        colliderData = circleData;
                        break;
                    default:
                        throw new System.NotImplementedException(mapCollider.GetType().Name);
                }
                
                colliderData.Position = new SVector2(pos.x, pos.y);
                colliderData.Rotation = colliderTransform.rotation.eulerAngles.z;
                
                colliderList.Add(colliderData);
            }
            
            return colliderList;
        }
        
        [ShowInInspector]
        private string MapExportPath => $"{ConStr.MapDataRoot}{MapEditor.MapId}.json";
#if UNITY_EDITOR
        [Button("导出地图")]
        public void ExportMapToJson()
        {
            var jsonName = MapEditor.MapId;
            
            if (string.IsNullOrEmpty(jsonName))
            {
                Debug.LogError("Json name is empty");
                return;
            }
            
            var mapData = ExportMap();
            var json = ConfigStaticSerializer.SerializeJson(mapData);
            var path = MapExportPath;
            System.IO.File.WriteAllText(path, json);
            
            Debug.Log($"Export map to {path}");

            UnityEditor.AssetDatabase.Refresh();

        }
#endif
    }
}