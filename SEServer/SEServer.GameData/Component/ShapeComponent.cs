using System.Collections.Generic;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData.Data;

namespace SEServer.GameData.Component;

public class ShapeComponent : IComponent
{
    public CId Id { get; set; }
    public EId EntityId { get; set; }
    public List<ShapeData> Shapes { get; set; } = new List<ShapeData>();
}