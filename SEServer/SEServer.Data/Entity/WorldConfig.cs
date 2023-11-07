using SEServer.Data.Interface;

namespace SEServer.Data;

public class WorldConfig : IWorldConfig
{
    public ServerContainer ServerContainer { get; set; } = null!;
    public int FramePerSecond { get; set; } = 30;

    public void Init()
    {
        
    }

    public void Start()
    {
       
    }

    public void Stop()
    {
        
    }
}