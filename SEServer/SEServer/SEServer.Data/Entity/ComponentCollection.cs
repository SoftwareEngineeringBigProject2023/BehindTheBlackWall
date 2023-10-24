﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SEServer.Data;

public class ComponentCollection
{
    public World World { get; set; }
    public Dictionary<Type, IComponentArray> Components { get; } = new();

    public ComponentArray<T> GetComponentArray<T>() where T : IComponent, new()
    {
        var type = typeof(T);
        if (!Components.ContainsKey(type))
        {
            Components.Add(type, new ComponentArray<T>());
        }
        return (ComponentArray<T>) Components[type];
    }
    
    public IComponentArray GetComponentArray(Type type)
    {
        if (!Components.ContainsKey(type))
        {
            Components.Add(type, (IComponentArray) Activator.CreateInstance(typeof(ComponentArray<>).MakeGenericType(type)));
        }
        return Components[type];
    }
    
    public T AddComponent<T>(EId eId) where T : struct, IComponent
    {
        var componentArray = GetComponentArray<T>();
        if (componentArray.HasEntity(eId))
        {
            return componentArray.Get(eId);
        }
        
        return componentArray.CreateComponent(eId);
    }
    
    public T GetComponent<T>(EId eId) where T : struct, IComponent
    {
        var componentArray = GetComponentArray<T>();
        if (!componentArray.HasEntity(eId))
        {
            throw new ArgumentException($"Component of type {typeof(T).Name} not found.");
        }
        
        return componentArray.Get(eId);
    }
    
    /// <summary>
    /// 标记为待删除
    /// </summary>
    /// <param name="eId"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="ArgumentException"></exception>
    public void MarkAsToBeDelete<T>(EId eId) where T : struct, IComponent
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
                snapshot.AddDataPack(dataPack);
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
}