using System;
using System.Collections.Generic;
using System.Linq;
using SEServer.Data.Interface;

namespace SEServer.Data;

/// <summary>
/// 实体管理器，实体的核心部分
/// </summary>
public class EntityManager
{   
    public EntityArray Entities { get; } = new EntityArray();
    public ComponentCollection Components { get; } = new ComponentCollection();
    public SystemCollection Systems { get; } = new SystemCollection();
    public World World { get; private set; }

    public EntityManager(World world)
    {
        World = world;
        Entities.World = world;
        Components.World = world;
        Systems.World = world;
    }

    public void Init()
    {
        Entities.Init();
    }
    
    public Entity CreateEntity()
    {
        return Entities.CreateEntity();
    }
    
    public Entity CreateEntity(EId eId)
    {
        return Entities.CreateEntity(eId);
    }
    
    public void RemoveEntity(Entity entity)
    {
        Entities.MarkAsToBeDelete(entity.Id);
    }
    
    public void RemoveEntity(EId eId)
    {
        Entities.MarkAsToBeDelete(eId);
    }
    
    public Entity GetEntity(EId eId)
    {
        return Entities.GetEntity(eId);
    }
    
    public T GetOrAddComponent<T>(Entity entity) where T :IComponent, new()
    {
        if (Components.GetOrAddComponent<T>(entity.Id, out var component))
        {
            MarkAsDirty(entity);
        }

        return component;
    }

    public T? GetComponent<T>(Entity entity) where T :IComponent, new()
    {
        return Components.GetComponent<T>(entity.Id);
    }
    
    public T? GetComponent<T>(EId eId) where T :IComponent, new()
    {
        return Components.GetComponent<T>(eId);
    }
    
    public IComponent? GetComponent(Entity entity, Type type)
    {
        return Components.GetComponent(entity.Id, type);
    }
    
    public bool TryGetComponent<T>(Entity entity, out T component) where T :IComponent, new()
    {
        var c = Components.GetComponent<T>(entity.Id);
        if (c != null)
        {
            component = c;
            return true;
        }
        
        component = default!;
        return false;
    }
    
    public bool TryGetComponent(Entity entity, Type type, out IComponent component)
    {
        var c = Components.GetComponent(entity.Id, type);
        if (c != null)
        {
            component = c;
            return true;
        }
        
        component = default!;
        return false;
    }
    
    public void RemoveComponent<T>(Entity entity) where T :IComponent, new()
    {
        MarkAsDirty(entity);
        Components.MarkAsToBeDelete<T>(entity.Id);
    }
    
    public T GetSingleton<T>() where T :IComponent, new()
    {
        var entity = Entities.GetSingletonEntity();
        var component = GetComponent<T>(entity);
        if (component == null)
        {
            component = GetOrAddComponent<T>(entity);
        }
        
        return component;
    }

    public Snapshot GetSnapshot()
    {
        var snapshot = new Snapshot();
        snapshot.WorldId = World.Id;
        Entities.WriteToSnapshot(snapshot);
        Components.WriteToSnapshot(snapshot);
        return snapshot;
    }
    
    public void ApplySnapshot(Snapshot snapshot)
    {
        Entities.ReadFromSnapshot(snapshot);
        Components.ReadFromSnapshot(snapshot);
    }
    
    public ComponentDataCollection<T> GetComponentDataCollection<T>() where T : IComponent, new()
    {
        var collection = new ComponentDataCollection<T>();
        var componentArray = Components.GetComponentArray<T>();
        collection.Components.AddRange(componentArray.Components);

        return collection;
    }

    public ComponentDataCollection<T, T1> GetComponentDataCollection<T, T1>()
        where T : IComponent, new()
        where T1 : IComponent, new()
    {
        var collection = new ComponentDataCollection<T, T1>();

        var componentTArray =Components.GetComponentArray<T>();
        var componentT1Array = Components.GetComponentArray<T1>();
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
    
    public ComponentDataCollection<T, T1, T2> GetComponentDataCollection<T, T1, T2>()
        where T : IComponent, new()
        where T1 : IComponent, new()
        where T2 : IComponent, new()
    {
        var collection = new ComponentDataCollection<T, T1, T2>();

        var componentTArray =Components.GetComponentArray<T>();
        var componentT1Array = Components.GetComponentArray<T1>();
        var componentT2Array = Components.GetComponentArray<T2>();
        var eIds = componentTArray.Components.Select(c => c.EntityId)
            .Intersect(componentT1Array.Components.Select(c => c.EntityId))
            .Intersect(componentT2Array.Components.Select(c => c.EntityId));
        
        collection.Components.AddRange(eIds.Select(
            eId =>
            {
                var c0 = componentTArray.Get(eId);
                var c1 = componentT1Array.Get(eId);
                var c2 = componentT2Array.Get(eId);
                return new ValueTuple<T, T1, T2>(c0, c1, c2);
            }));
        
        return collection;
    }
    
    public ComponentDataCollection<T, T1, T2, T3> GetComponentDataCollection<T, T1, T2, T3>()
        where T : IComponent, new()
        where T1 : IComponent, new()
        where T2 : IComponent, new()
        where T3 : IComponent, new()
    {
        var collection = new ComponentDataCollection<T, T1, T2, T3>();

        var componentTArray =Components.GetComponentArray<T>();
        var componentT1Array = Components.GetComponentArray<T1>();
        var componentT2Array = Components.GetComponentArray<T2>();
        var componentT3Array = Components.GetComponentArray<T3>();
        var eIds = componentTArray.Components.Select(c => c.EntityId)
            .Intersect(componentT1Array.Components.Select(c => c.EntityId))
            .Intersect(componentT2Array.Components.Select(c => c.EntityId))
            .Intersect(componentT3Array.Components.Select(c => c.EntityId));
        
        collection.Components.AddRange(eIds.Select(
            eId =>
            {
                var c0 = componentTArray.Get(eId);
                var c1 = componentT1Array.Get(eId);
                var c2 = componentT2Array.Get(eId);
                var c3 = componentT3Array.Get(eId);
                return new ValueTuple<T, T1, T2, T3>(c0, c1, c2, c3);
            }));
        
        return collection;
    }
    
    public ComponentDataCollection<T, T1, T2, T3, T4> GetComponentDataCollection<T, T1, T2, T3, T4>()
        where T : IComponent, new()
        where T1 : IComponent, new()
        where T2 : IComponent, new()
        where T3 : IComponent, new()
        where T4 : IComponent, new()
    {
        var collection = new ComponentDataCollection<T, T1, T2, T3, T4>();

        var componentTArray =Components.GetComponentArray<T>();
        var componentT1Array = Components.GetComponentArray<T1>();
        var componentT2Array = Components.GetComponentArray<T2>();
        var componentT3Array = Components.GetComponentArray<T3>();
        var componentT4Array = Components.GetComponentArray<T4>();
        var eIds = componentTArray.Components.Select(c => c.EntityId)
            .Intersect(componentT1Array.Components.Select(c => c.EntityId))
            .Intersect(componentT2Array.Components.Select(c => c.EntityId))
            .Intersect(componentT3Array.Components.Select(c => c.EntityId))
            .Intersect(componentT4Array.Components.Select(c => c.EntityId));
        
        collection.Components.AddRange(eIds.Select(
            eId =>
            {
                var c0 = componentTArray.Get(eId);
                var c1 = componentT1Array.Get(eId);
                var c2 = componentT2Array.Get(eId);
                var c3 = componentT3Array.Get(eId);
                var c4 = componentT4Array.Get(eId);
                return new ValueTuple<T, T1, T2, T3, T4>(c0, c1, c2, c3, c4);
            }));
        
        return collection;
    }

    public void MarkAsDirty(Entity entity)
    {
        Entities.MarkAsDirty(entity);
    }
    
    public void MarkAsDirty(INetComponent component)
    {
        Components.MarkAsDirty(component);
        Components.MarkAsChanged(component);
    }
    
    /// <summary>
    /// 标记为更改
    /// 仅用于本地组件
    /// </summary>
    /// <param name="component"></param>
    public void MarkAsChanged(IComponent component)
    {
        Components.MarkAsChanged(component);
    }

    /// <summary>
    /// 标记为脏
    /// 用于网络传输
    /// </summary>
    /// <param name="component"></param>
    /// <returns></returns>
    public bool IsChanged(IComponent component)
    {
        return Components.IsChanged(component);
    }
}