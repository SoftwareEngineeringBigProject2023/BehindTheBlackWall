using SEServer.Data.Interface;
using UnityEngine;

namespace Game.Controller
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