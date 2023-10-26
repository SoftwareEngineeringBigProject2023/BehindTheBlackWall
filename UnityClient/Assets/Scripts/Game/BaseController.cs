using SEServer.Data;
using UnityEngine;

namespace Game
{
    public abstract class BaseController : MonoBehaviour
    {
        public EntityMapperManager MapperManager { get; set; } = null!;
        public EntityMapperManager.ComponentMapper ComponentMapper { get; set; }
        
        public T GetEComponent<T>() where T : IComponent, new()
        {
            return MapperManager.GetEComponent<T>(ComponentMapper);
        }
    }
}