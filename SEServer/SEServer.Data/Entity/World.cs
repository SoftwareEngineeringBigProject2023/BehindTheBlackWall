﻿using System;
using System.Linq;
using SEServer.Data.Interface;

namespace SEServer.Data;

public abstract class World
{
    public WId Id { get; set; }
    public EntityManager EntityManager { get; }
    public TimeManager Time { get; } = new TimeManager();
    public ServerContainer ServerContainer { get; set; } = null!;
    public WorldSetting WorldSetting { get; set; } = new WorldSetting();
    
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
    
    public void MarkAsDirty(Entity entity)
    {
        EntityManager.MarkAsDirty(entity);
    }
    
    public void MarkAsDirty(INetComponent component)
    {
        EntityManager.MarkAsDirty(component);
    }
}