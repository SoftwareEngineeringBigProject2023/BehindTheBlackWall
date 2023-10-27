using System.Collections.Generic;

namespace SEServer.Data;

public class SystemProvider : ISystemProvider
{
    public ServerContainer ServerContainer { get; set; } = null!;
    
    public List<ISystem> Systems { get; set; } = new();
    public void Init()
    {
        
    }

    public void Start()
    {
        
    }

    public void Stop()
    {
        
    }

    public void AddSystem(ISystem system)
    {
        Systems.Add(system);
    }

    public List<ISystem> GetAllSystems()
    {
        return Systems;
    }
}