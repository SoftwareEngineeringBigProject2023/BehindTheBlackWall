using System;
using System.Collections.Generic;
using System.Linq;
using SEServer.Data.Interface;

namespace SEServer.Data;

public class ComponentCollection
{
    public World World { get; set; } = null!;
    public Dictionary<Type, IComponentArray> Components { get; } = new();

    public ComponentArray<T> GetComponentArray<T>() where T : IComponent, new()
    {
        var type = typeof(T);
        if (!Components.ContainsKey(type))
        {
            var componentArray = new ComponentArray<T>();
            componentArray.World = World;
            Components.Add(type, componentArray);
        }
        return (ComponentArray<T>) Components[type];
    }
    
    public IComponentArray GetComponentArray(Type type)
    {
        if (!Components.ContainsKey(type))
        {
            var obj = Activator.CreateInstance(typeof(ComponentArray<>).MakeGenericType(type));
            if (obj is not IComponentArray componentArray)
            {
                throw new Exception($"Create instance of {type.Name} failed.");
            }
            
            componentArray.World = World;
            Components.Add(type, componentArray);
        }
        return Components[type];
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eId"></param>
    /// <param name="component"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>是否是创建的组件</returns>
    public bool GetOrAddComponent<T>(EId eId,out T component) where T : IComponent, new()
    {
        var componentArray = GetComponentArray<T>();
        if (componentArray.HasEntity(eId))
        {
            component = componentArray.Get(eId);
            return false;
        }

        component = componentArray.CreateComponent(eId);
        return true;
    }
    
    public bool HasComponent<T>(EId eId) where T : IComponent, new()
    {
        var componentArray = GetComponentArray<T>();
        return componentArray.HasEntity(eId);
    }
    
    public T? GetComponent<T>(EId eId) where T : IComponent, new()
    {
        var componentArray = GetComponentArray<T>();
        if (!componentArray.HasEntity(eId))
        {
            return default;
        }
        
        return componentArray.Get(eId);
    }
    
    public IComponent? GetComponent(EId entityId, Type type)
    {
        var componentArray = GetComponentArray(type);
        if (!componentArray.HasEntity(entityId))
        {
            return null!;
        }
        
        return componentArray.GetI(entityId);
    }

    /// <summary>
    /// 标记为待删除
    /// </summary>
    /// <param name="eId"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="ArgumentException"></exception>
    public void MarkAsToBeDelete<T>(EId eId) where T : IComponent, new()
    {
        var componentArray = GetComponentArray<T>();
        if (!componentArray.HasEntity(eId))
        {
            throw new ArgumentException($"Component of type {typeof(T).Name} not found.");
        }
        
        componentArray.MarkAsToBeDelete(eId);
    }
    
    /// <summary>
    /// 移除所有标记的组件   
    /// </summary>
    public void RemoveMarkComponents()
    {
        foreach (var componentArray in Components.Values)
        {
            componentArray.RemoveMarkComponents();
        }
    }

    public void MarkAsToBeDeleteAll(IEnumerable<EId> eIds)
    {
        var eIdsArray = eIds.ToArray();
        foreach (var pair in Components)
        {
            var componentArray = pair.Value;
            foreach (var eId in eIdsArray)
            {
                componentArray.MarkAsToBeDelete(eId);
            }
        }
    }

    public void ClearDirty()
    {
        foreach (var pair in Components)
        {
            var componentArray = pair.Value;
            componentArray.ClearDirty();
            componentArray.ClearChanged();
        }
    }

    /// <summary>
    /// 将组件列表写入快照
    /// 只写入实现了INetComponent的组件
    /// </summary>
    /// <param name="snapshot"></param>
    public void WriteToSnapshot(Snapshot snapshot)
    {
        foreach (var pair in Components)
        {
            var componentArray = pair.Value;
            if(componentArray.ContainInterface(typeof(INetComponent)))
            {
                var dataPack = componentArray.WriteToDataPack(World.ServerContainer.Get<IComponentSerializer>());
                snapshot.ComponentArrayDataPacks.Add(dataPack);
            }
        }
    }

    /// <summary>
    /// 从快照中读取组件列表
    /// </summary>
    /// <param name="snapshot"></param>
    public void ReadFromSnapshot(Snapshot snapshot)
    {
        foreach (var dataPack in snapshot.ComponentArrayDataPacks)
        {
            var serializer = World.ServerContainer.Get<IComponentSerializer>();
            var componentArray = GetComponentArray(serializer.GetTypeByCode(dataPack.TypeCode));
            componentArray.ReadFromDataPack(serializer, dataPack);
        }
    }

    public void MarkAsDirty(INetComponent component)
    {
        var componentArray = GetComponentArray(component.GetType());
        componentArray.MarkAsDirty(component);
    }

    public void MarkAsChanged(IComponent component)
    {
        var componentArray = GetComponentArray(component.GetType());
        componentArray.MarkAsChanged(component);
    }
    
    public bool IsChanged(IComponent component)
    {
        var componentArray = GetComponentArray(component.GetType());
        return componentArray.IsChanged(component);
    }
}