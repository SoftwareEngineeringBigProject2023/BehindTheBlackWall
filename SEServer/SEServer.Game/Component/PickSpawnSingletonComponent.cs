using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData.CTData;

namespace SEServer.Game.Component;

public class PickSpawnSingletonComponent : IComponent
{
    public CId Id { get; set; }
    public EId EntityId { get; set; }
    public List<PickSpawnData> PickSpawnDataList { get; set; } = new List<PickSpawnData>();
}

public class PickSpawnData
{
    public SpecialPointData BindSpecialPoint { get; set; } = null!;
    public bool HasPickData { get; set; }
    public float PickDataSpawnCooldown { get; set; }
}