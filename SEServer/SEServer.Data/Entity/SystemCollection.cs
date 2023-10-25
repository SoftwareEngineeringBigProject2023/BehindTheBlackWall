using System.Collections.Generic;

namespace SEServer.Data;

public class SystemCollection
{
    public List<ISystem> SystemList { get; } = new List<ISystem>();
    public World World { get; set; } = null!;

    public void UpdateAll()
    {
        foreach (var system in SystemList)
        {
            system.Update();
        }
    }

    public void AddSystems(List<ISystem> systems)
    {
        SystemList.AddRange(systems);
    }
}