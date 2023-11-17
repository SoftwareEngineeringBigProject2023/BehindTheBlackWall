using SEServer.Data;

namespace SEServer.Game;

public class ForceData
{
    public EId TargetId { get; set; }
    public SVector2 Force { get; set; }
}