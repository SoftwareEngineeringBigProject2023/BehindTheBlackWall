using System.Collections.Generic;
using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData.Component;

public class BulletComponent : IComponent
{
    public CId Id { get; set; }
    public EId EntityId { get; set; }
    public int CreatorId { get; set; }
    public float Damage { get; set; }
    public float Speed { get; set; }
    public Dictionary<string, int> Properties { get; set; } = new();
    public int MarkDeleteDelayStep { get; set; } = -1;
    public int SkipMove { get; set; } = 0;
}