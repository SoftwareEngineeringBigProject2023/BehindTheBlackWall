using SEServer.Data;

namespace SEServer.Core;

public class ServerWorldConfig : WorldConfig
{
    public int VelocityIterations { get; set; } = 8;
    public int PositionIterations { get; set; } = 3;
}