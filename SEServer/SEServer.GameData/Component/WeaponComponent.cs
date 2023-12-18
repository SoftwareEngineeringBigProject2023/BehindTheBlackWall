using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData.CTData;

namespace SEServer.GameData.Component;

public class WeaponComponent : IComponent
{
    public CId Id { get; set; }
    public EId EntityId { get; set; }
    public string WeaponId { get; set; } = "";
    public WeaponCTData? WeaponCTData { get; set; }
    public float WeaponShootCooldown { get; set; }
}