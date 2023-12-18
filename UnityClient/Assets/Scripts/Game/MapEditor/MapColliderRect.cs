using System;
using Game.Utils;
using SEServer.Data;
using SEServer.GameData.CTData;
using SEServer.GameData.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.MapEditor
{
    public class MapColliderRect : MapColliderBase
    {
        public Vector2 size = Vector2.one;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var rotate = transform.rotation.eulerAngles.z;
            var position = transform.position;
            // DrawLine
            var leftTop = position + (Vector3)GetPointAround(rotate, new Vector2(-size.x / 2, size.y / 2));
            var rightTop = position + (Vector3)GetPointAround(rotate, new Vector2(size.x / 2, size.y / 2));
            var leftBottom = position + (Vector3)GetPointAround(rotate, new Vector2(-size.x / 2, -size.y / 2));
            var rightBottom = position + (Vector3)GetPointAround(rotate, new Vector2(size.x / 2, -size.y / 2));
            Gizmos.DrawLine(leftTop, rightTop);
            Gizmos.DrawLine(rightTop, rightBottom);
            Gizmos.DrawLine(rightBottom, leftBottom);
            Gizmos.DrawLine(leftBottom, leftTop);
        }

        private Vector2 GetPointAround(float rotate, Vector2 point)
        {
            var angle = Vector2.Angle(Vector2.right, point) * Mathf.Deg2Rad;
            if (point.y < 0)
                angle = -angle;
            
            var radRotate = rotate * Mathf.Deg2Rad;
            var length = point.magnitude;
            angle += radRotate;
            return new Vector2((float) Math.Cos(angle) * length, (float) Math.Sin(angle) * length);
        }
        
        [Button("使用物体大小")]
        private void UseObjectSize()
        {
            var spriteSize = GetComponent<SpriteRenderer>().sprite.rect.size;
            var spritePerPixel = GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            spriteSize /= spritePerPixel;
            var localSize = transform.localScale;
            this.size = new Vector2(spriteSize.x * localSize.x, spriteSize.y * localSize.y);
        }

        public void SetData(ColliderDataRect rect)
        {
            size = rect.Size.ToUVector2();
            transform.eulerAngles = new Vector3(0, 0, rect.Rotation);
            transform.position = rect.Position.ToUVector2();
        }
    }
}