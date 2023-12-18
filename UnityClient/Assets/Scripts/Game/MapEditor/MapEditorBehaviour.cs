using System;
using System.Collections.Generic;
using Game.Framework;
using SEServer.Data;
using SEServer.GameData.CTData;
using SEServer.GameData.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.MapEditor
{
    public class MapEditorBehaviour : MonoBehaviour
    {
        public GameMapCTData CurMapData { get; set; }
        
        public string MapId;
        public string MapName;
        
        public Transform BackgroundRoot;
        public Transform DecorationRoot;
        public Transform ColliderEditorRoot;
        public Transform SpecialPointRoot;
        
        public Vector2 MapEdgeSize;
        
        [ShowInInspector]
        private SpriteRenderer _sampleMapBg;

        public void ClearMap()
        {
            foreach (Transform child in BackgroundRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in DecorationRoot)
            {
                Destroy(child.gameObject);
            }
            
            foreach (Transform child in ColliderEditorRoot)
            {
                Destroy(child.gameObject);
            }
            
            foreach (Transform child in SpecialPointRoot)
            {
                Destroy(child.gameObject);
            }
        }

        [Button("使用背景图边界")]
        private void UseBgEdge()
        {
            if (_sampleMapBg == null)
                return;
            
            var spriteSize = _sampleMapBg.sprite.rect.size;
            var spritePerPixel = _sampleMapBg.sprite.pixelsPerUnit;
            spriteSize /= spritePerPixel;
            var localSize = _sampleMapBg.transform.localScale;
            MapEdgeSize = new Vector2(spriteSize.x * localSize.x, spriteSize.y * localSize.y);
        }
        
        private void OnDrawGizmos()
        {
            // Draw map edge
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, MapEdgeSize);
        }
    }
}