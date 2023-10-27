using System.Collections.Generic;
using SEServer.Data.Interface;

namespace SEServer.Data;

public class SystemProvider : ISystemProvider
{
    public ServerContainer ServerContainer { get; set; } = null!;
    
    public List<ISystem> Systems { get; set; } = new();
    public void Init()
    {
        // 对System基于PriorityAttribute，从大到小排序
        var priorityDic = new Dictionary<ISystem, int>();
        foreach (var system in Systems)
        {
            var priority = system.GetType().GetCustomAttributes(typeof(PriorityAttribute), false);
            if (priority.Length == 0)
            {
                priorityDic.Add(system, 0);
            }
            else
            {
                priorityDic.Add(system, ((PriorityAttribute) priority[0]).Priority);
            }
        }
        
        // 从大到小排序
        Systems.Sort((a, b) =>
        {
            var aPriority = priorityDic[a];
            var bPriority = priorityDic[b];
            return bPriority.CompareTo(aPriority);
        });
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