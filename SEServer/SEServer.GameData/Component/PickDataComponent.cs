using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData.Component;

public class PickDataComponent : IComponent
{
    public CId Id { get; set; }
    public EId EntityId { get; set; }
    public int BindIndex { get; set; }
    public int Score { get; set; } = 20;
}