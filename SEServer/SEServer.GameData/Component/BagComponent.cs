using System.Collections.Generic;
using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData.Component;

public class BagComponent : IComponent
{
    public CId Id { get; set; }
    public EId EntityId { get; set; }
    public List<BagItem> Items { get; set; } = new List<BagItem>();
    public List<BagItem> Weapons { get; set; } = new List<BagItem>();
    public int SelectedWeaponIndex { get; set; } = 0;
    public bool NeedRefresh { get; set; } = false;
}