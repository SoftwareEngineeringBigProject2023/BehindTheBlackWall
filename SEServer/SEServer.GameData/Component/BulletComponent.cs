using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData.Component;

public class BulletComponent : IComponent
{
    public CId Id { get; set; }
    public EId EntityId { get; set; }
    public int CreatorId { get; set; }
    public int Damage { get; set; }
}