using System;
using System.Linq;

namespace SEServer.Data;

public abstract class World
{
    public WId Id { get; set; }
    public EntityManager EntityManager { get; }
    public TimeManager Time { get; } = new TimeManager();
    public ServerContainer ServerContainer { get; set; } = null!;
    
    public World()
    {
        EntityManager = new EntityManager(this);
    }
    
    public void Init()
    {
        EntityManager.Init();
        
        // 注入系统
        var systems = ServerContainer.Get<ISystemProvider>();
        EntityManager.Systems.AddSystems(systems.GetAllSystems());
        
        // 初始化系统
        EntityManager.Systems.InitAll();
    }

    /// <summary>
    /// 世界帧更新
    /// </summary>
    /// <param name="deltaTime"></param>
    public abstract void Update(float deltaTime);
}