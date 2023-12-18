using System;
using Game.Utils;
using SEServer.GameData.CTData;
using UnityEngine;

namespace Game.MapEditor
{
    public class MapSpecialPoint : MonoBehaviour
    {
        public string pointId;
        public int pointType;

        private void OnDrawGizmos()
        {
            switch (pointType)
            {
                case 0:
                    Gizmos.color = Color.green;
                    break;
                case 1:
                    Gizmos.color = Color.red;
                    break;
                case 2:
                    Gizmos.color = new Color(1f, 0.5f, 0.5f);
                    break;
                default:
                    Gizmos.color = Color.magenta;
                    break;
            }
            Gizmos.DrawSphere(transform.position, 0.5f);
            Gizmos.color = Color.white;
            // 输出文本
#if UNITY_EDITOR
            UnityEditor.Handles.Label(transform.position, pointId);
#endif
        }

        public void SetData(SpecialPointData specialPoint)
        {
            pointId = specialPoint.Id;
            pointType = specialPoint.Type;
        }
    }
}