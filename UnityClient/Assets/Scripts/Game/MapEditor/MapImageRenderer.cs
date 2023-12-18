using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.MapEditor
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MapImageRenderer : MonoBehaviour
    {
        [AssetsOnly]
        [OnValueChanged("OnSpriteChanged")]
        public Sprite sprite;
        
        [ReadOnly]
        public string imagePath;

        public void SetSprite(Sprite setSprite)
        {
            this.sprite = setSprite;
            GetComponent<SpriteRenderer>().sprite = setSprite;
        }
#if UNITY_EDITOR
        private void OnSpriteChanged()
        {

            if (sprite != null)
            {
                var assetPath = UnityEditor.AssetDatabase.GetAssetPath(sprite);
                imagePath = assetPath;
            }
            else
            {
                imagePath = "";
            }
            
            SetSprite(sprite);


            UnityEditor.Undo.RecordObject(this, "Change Sprite");
            UnityEditor.Undo.RecordObject(GetComponent<SpriteRenderer>(), "Change Sprite");

        }
#endif
    }
}