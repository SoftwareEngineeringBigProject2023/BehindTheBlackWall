using System;
using System.Collections.Generic;
using SEServer.Data.Interface;

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

        foreach (var system in systems)
        {
            system.World = World;
        }
    }

    public void InitAll()
    {
        foreach (var system in SystemList)
        {
            try
            {
                system.Init();
            }
            catch (Exception)
            {
                World.ServerContainer.Get<ILogger>().LogError($"初始化系统{system.GetType().Name}失败");
                throw;
            }
        }
    }
}