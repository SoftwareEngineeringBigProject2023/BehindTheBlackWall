using System;
using Game.Utils;
using SEServer.GameData.CTData;
using SEServer.GameData.Data;
using UnityEngine;

namespace Game.MapEditor
{
    public class MapColliderCircle : MapColliderBase
    {
        public float radius = 1;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public void SetData(ColliderDataCircle circle)
        {
            radius = circle.Radius;
            transform.eulerAngles = new Vector3(0, 0, circle.Rotation);
            transform.position = circle.Position.ToUVector2();
        }
    }
}