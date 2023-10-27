using System;
using UnityEngine;

namespace Game
{
    public class GraphAttachBehaviour : MonoBehaviour, IClientAttachBehaviour
    {
        public ClientBehaviour ClientBehaviour { get; set; }

        public Transform GraphRoot { get; set; }
        
        private void Awake()
        {
            var graphRoot = new GameObject("GraphRoot");
            GraphRoot = graphRoot.transform;
        }

        public void UpdateBehaviour()
        {
            
        }
    }
}