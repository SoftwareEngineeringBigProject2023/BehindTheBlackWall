using SEServer.Data;
using SEServer.GameData.Component;
using IComponent = SEServer.Data.Interface.IComponent;

namespace Game.Component
{
    public class LocalPlayerInfoSingletonComponent : IComponent
    {
        public CId Id { get; set; }
        public EId EntityId { get; set; }

        public EId PlayerEntityId { get; set; } = EId.Invalid;
    }
}