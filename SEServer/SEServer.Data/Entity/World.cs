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

    /// <summary>
    /// 世界帧更新
    /// </summary>
    /// <param name="deltaTime"></param>
    public abstract void Update(float deltaTime);

    public ComponentDataCollection<T> GetComponentDataCollection<T>() where T : IComponent, new()
    {
        var collection = new ComponentDataCollection<T>();
        var componentArray = EntityManager.Components.GetComponentArray<T>();
        collection.Components.AddRange(componentArray.Components);

        return collection;
    }

    public ComponentDataCollection<T, T1> GetComponentDataCollection<T, T1>()
        where T : IComponent, new()
        where T1 : IComponent, new()
    {
        var collection = new ComponentDataCollection<T, T1>();

        var componentTArray = EntityManager.Components.GetComponentArray<T>();
        var componentT1Array = EntityManager.Components.GetComponentArray<T1>();
        var eIds = componentTArray.Components.Select(c => c.EntityId)
            .Intersect(componentT1Array.Components.Select(c => c.EntityId));
        
        collection.Components.AddRange(eIds.Select(
            eId =>
            {
                var c0 = componentTArray.Get(eId);
                var c1 = componentT1Array.Get(eId);
                return new ValueTuple<T, T1>(c0, c1);
            }));
        
        return collection;
    }
}