using SEServer.Data;
using SEServer.Data.Interface;
using UnityEngine;

namespace Game.GameComponent
{
    public class GraphInstanceComponent : IComponent
    {
        public CId Id { get; set; }
        public EId EntityId { get; set; }
        
        public Transform GraphRoot { get; set; }
    }
}