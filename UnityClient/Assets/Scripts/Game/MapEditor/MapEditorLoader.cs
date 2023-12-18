using System.Collections.Generic;
using Game.Framework;
using Game.Utils;
using SEServer.Data;
using SEServer.GameData.CTData;
using SEServer.GameData.Data;
using UnityEngine;

namespace Game.MapEditor
{
    public class MapEditorLoader : MonoBehaviour
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
        
        public void LoadMap(GameMapCTData mapData)
        {
            MapEditor.CurMapData = mapData;
            
            MapEditor.ClearMap();
            
            LoadImage(MapEditor.BackgroundRoot, mapData.Backgrounds, -10);
            LoadImage(MapEditor.DecorationRoot, mapData.Decorations, 5);
            LoadCollider(MapEditor.ColliderEditorRoot, mapData.Colliders);
            LoadSpecialPoint(MapEditor.SpecialPointRoot, mapData.SpecialPoints);
            MapEditor.MapEdgeSize = mapData.MapSize.ToUVector2();
            MapEditor.MapId = mapData.PrimaryKey;
            MapEditor.MapName = mapData.DisplayName;
        }

        private void LoadSpecialPoint(Transform specialPointRoot, List<SpecialPointData> specialPoints)
        {
            foreach (var specialPoint in specialPoints)
            {
                var go = new GameObject($"SpecialPoint - {specialPoint.Id}");
                go.transform.SetParent(specialPointRoot);

                var specialPointRenderer = go.AddComponent<MapSpecialPoint>();
                specialPointRenderer.SetData(specialPoint);
                
                var specialPointTransform = go.transform;
                specialPointTransform.localPosition = specialPoint.Position.ToUVector2();
            }
        }

        private void LoadImage(Transform root, List<ImageData> images, int layer = 0)
        {
            foreach (var image in images)
            {
                var go = new GameObject($"Image - {image.Path}");
                go.transform.SetParent(root);

                var sprite = GameManager.Res.LoadAsset<Sprite>(image.Path);
                if (sprite == null)
                {
                    Debug.LogError($"Load sprite failed: {image.Path}");
                    continue;
                }
                
                var imageRenderer = go.AddComponent<MapImageRenderer>();
                imageRenderer.SetSprite(sprite);
                
                var imageTransform = go.transform;
                imageTransform.localPosition = image.Position.ToUVector2();
                imageTransform.localScale = image.Size.ToUVector2();
                imageTransform.localRotation = Quaternion.Euler(0, 0, image.Rotation);
                
                var spriteRenderer = go.GetComponent<SpriteRenderer>();
                spriteRenderer.color = image.Color.ToColor();
                spriteRenderer.sortingOrder = layer;
            }
        }
        
        private void LoadCollider(Transform root, List<ColliderDataBase> colliders)
        {
            foreach (var colliderData in colliders)
            {
                switch (colliderData)
                {
                    case ColliderDataRect rect:
                    {
                        var go = new GameObject($"Collider - Rect");
                        go.transform.SetParent(root);
                        var mapCollider = go.AddComponent<MapColliderRect>();
                        mapCollider.SetData(rect);
                        break;
                    }
                    case ColliderDataCircle circle:
                    {
                        var go = new GameObject($"Collider - Circle");
                        go.transform.SetParent(root);
                        var mapCollider = go.AddComponent<MapColliderCircle>();
                        mapCollider.SetData(circle);
                        break;
                    }
                }
            }
        }
        
        public void LoadMapFromJson(string json)
        {
            var mapData = ConfigStaticDeserializer.ParseJson<GameMapCTData>(json);
            LoadMap(mapData);
        }
    }
}