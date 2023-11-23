using SEServer.Data;

namespace SEServer.Core;

public class NetConfig : INetConfig
{
    public ServerContainer ServerContainer { get; set; }
    public void Init()
    {
        
    }

    public void Start()
    {
        
    }

    public void Stop()
    {
        
    }

    public string[] ListenUrls { get; set; } = null!;
}