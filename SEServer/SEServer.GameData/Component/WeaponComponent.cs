using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData.Component;

public class WeaponComponent : IComponent
{
    public CId Id { get; set; }
    public EId EntityId { get; set; }
    public int WeaponType { get; set; }
    public float WeaponShootCooldown { get; set; }
}