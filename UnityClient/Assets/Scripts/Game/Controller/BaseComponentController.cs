using SEServer.Data.Interface;

namespace Game.Controller
{
    public abstract class BaseComponentController : BaseController
    {
        public ControllerMapper ControllerMapper { get; set; }
        
        public T GetEComponent<T>() where T : INetComponent, new()
        {
            return MapperManager.GetEComponent<T>(ControllerMapper);
        }

        public T GetEController<T>() where T : BaseController
        {
            return MapperManager.GetEController<T>(ControllerMapper);
        }
    }
}