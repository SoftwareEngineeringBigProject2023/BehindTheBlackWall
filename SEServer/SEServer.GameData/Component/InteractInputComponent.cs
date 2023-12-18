using MessagePack;
using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData.Component;

[MessagePackObject]
public class InteractInputComponent : IC2SComponent
{
    [Key(0)]
    public CId Id { get; set; }
    [Key(1)]
    public EId EntityId { get; set; }
    [Key(2)]
    public PlayerId Owner { get; set; } = PlayerId.Invalid;
    [Key(3)]
    public int SelectedWeaponIndex { get; set; }
}