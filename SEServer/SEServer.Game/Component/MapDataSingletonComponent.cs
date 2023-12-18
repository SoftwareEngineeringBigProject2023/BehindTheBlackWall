using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData.CTData;

namespace SEServer.Game.Component;

public class MapDataSingletonComponent : IComponent
{
    public CId Id { get; set; }
    public EId EntityId { get; set; }
    public GameMapCTData MapCTData { get; set; } = null!;
}