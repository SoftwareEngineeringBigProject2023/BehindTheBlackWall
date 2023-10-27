using System;
using System.Collections.Generic;
using System.Linq;

namespace SEServer.Data;

/// <summary>
/// 组件列表
/// </summary>
public class ComponentArray<T> : IComponentArray where T : IComponent, new()
{
    private int IdAutoIncrement { get; set; } = 1;
    private Dictionary<EId, CId> EntityToComponents { get; } = new();
    private Dictionary<CId, EId> ComponentToEntity { get; } = new();
    private Dictionary<CId, int> ComponentToIndex { get; } = new();
    private HashSet<EId> DeleteEntities { get; } = new();
    private List<Type> Interfaces { get; } = new();
    
    public List<T> Components { get; } = new();

    public ComponentArray()
    {
        // 缓存接口
        var type = typeof(T);
        var interfaces = type.GetInterfaces();
        foreach (var iType in interfaces)
        {
            Interfaces.Add(iType);
        }
    }

    public bool ContainInterface(Type iType)
    {
        if(Interfaces.Contains(iType))
            return true;
        
        return false;
    }

    public Type GetDataType()
    {
        return typeof(T);
    }

    private CId CreateId()
    {
        CId id = new CId()
        {
            Id = IdAutoIncrement++
        };
        return id;
    }
    
    public T CreateComponent(EId eId)
    {
        T component = new();
        component.Id = CreateId();
        

        return component;
    }
    
    public void AddComponent(T component)
    {
        Components.Add(component);
        ComponentToIndex.Add(component.Id, Components.Count - 1);
        ComponentToEntity.Add(component.Id, component.EntityId);
        EntityToComponents.Add(component.EntityId, component.Id);
    }

    /// <summary>
    /// 将标记的实体从组件列表中移除
    /// </summary>
    public void RemoveMarkComponents()
    {
        foreach (var eId in DeleteEntities)
        {
            var cId = EntityToComponents[eId];
            var index = ComponentToIndex[cId];
            Components.RemoveAt(index);
        }
        // 重建索引
        RebuildIndex();

        DeleteEntities.Clear();
    }

    private void RebuildIndex()
    {
        ComponentToIndex.Clear();
        ComponentToEntity.Clear();
        EntityToComponents.Clear();
        for (int i = 0; i < Components.Count; i++)
        {
            ComponentToIndex.Add(Components[i].Id, i);
            ComponentToEntity.Add(Components[i].Id, Components[i].EntityId);
            EntityToComponents.Add(Components[i].EntityId, Components[i].Id);
        }
    }

    public bool HasEntity(EId eId)
    {
        return EntityToComponents.ContainsKey(eId);
    }

    public T Get(EId eId)
    {
        return Components[ComponentToIndex[EntityToComponents[eId]]];
    }

    /// <summary>
    /// 将实体的组件标记为删除
    /// </summary>
    /// <param name="eId"></param>
    public void MarkAsToBeDelete(EId eId)
    {
        DeleteEntities.Add(eId);
    }

    public void ClearDirty()
    {
        foreach (var component in Components)
        {
            if(component is INetComponent netComponent)
                netComponent.IsDirty = false;
        }
    }
    
    public ComponentArrayDataPack WriteToDataPack(IComponentSerializer serializer)
    {
        var components = new List<T>();
        components.AddRange(Components);
        var dataPack = serializer.Serialize(components);
        return dataPack;
    }

    public void ReadFromDataPack(IComponentSerializer serializer, ComponentArrayDataPack dataPack)
    {
        Components.Clear();
        EntityToComponents.Clear();
        ComponentToIndex.Clear();

        var components = serializer.Deserialize<T>(dataPack);
        for (var index = 0; index < components.Count; index++)
        {
            var component = components[index];
            Components.Add(component);
        }
        
        // 重建索引
        RebuildIndex();
    }

    public ComponentArrayDataPack WriteChangedToDataPack(IComponentSerializer serializer, List<EId> includeEIds, PlayerId player)
    {
        var components = new List<T>();
        
        foreach (var component in Components)
        {
            var netComponent = (INetComponent)component;
            if (!netComponent.IsDirty)
            {
                continue;
            }
            
            if (!includeEIds.Contains(component.EntityId))
            {
                continue;
            }

            // ReSharper disable once SuspiciousTypeConversion.Global
            if (component is IC2SComponent c2SComponent)
            {
                if(c2SComponent.Owner != PlayerId.Invalid && player != PlayerId.Invalid && c2SComponent.Owner != player )
                {
                    continue;
                }
            }
            
            Components.Add(component);
        }
        
        var dataPack = serializer.Serialize(components);
        return dataPack;
    }

    public void ReadChangedFromDataPack(IComponentSerializer serializer, ComponentArrayDataPack dataPack)
    {
        var components = serializer.Deserialize<T>(dataPack);
        for (var index = 0; index < components.Count; index++)
        {
            var component = components[index];
            var hasComponent = ComponentToIndex.ContainsKey(component.Id);
            if (hasComponent)
            {
                Components[ComponentToIndex[component.Id]] = component;
            }
            else
            {
                AddComponent(component);
            }
        }
    }

    public ComponentNotifyMessageDataPack WriteChangedToNotifyDataPack(IComponentSerializer serializer, List<EId> includeEIds)
    {
        var componentNotifyDataPack = new ComponentNotifyMessageDataPack();
        componentNotifyDataPack.TypeCode = serializer.GetCodeByType(typeof(T));
        foreach (var component in Components)
        {
            var notifyComponent = (INotifyComponent)component;
            if (notifyComponent.IsDirty || includeEIds.Contains(component.EntityId))
            {
                componentNotifyDataPack.AddNotifyMessage(notifyComponent.Id, notifyComponent.TakeAllNotifyMessages());
            }
        }
        
        return componentNotifyDataPack;
    }

    public void ReadChangedFromNotifyDataPack(IComponentSerializer serializer, ComponentNotifyMessageDataPack dataPack)
    {
        foreach (var component in Components)
        {
            var notifyComponent = (INotifyComponent)component;
            if (dataPack.NotifyMessages.TryGetValue(component.Id, out var notifyMessages))
            {
                notifyComponent.ReceiveNotifyMessages(notifyMessages);
            }
        }
    }

    public ComponentSubmitMessageDataPack WriteChangedToSubmitDataPack(IComponentSerializer serializer, List<EId> includeEIds, PlayerId player)
    {
        var componentNotifyDataPack = new ComponentSubmitMessageDataPack();
        componentNotifyDataPack.TypeCode = serializer.GetCodeByType(typeof(T));
        foreach (var component in Components)
        {
            var submitComponent = (ISubmitComponent)component;
            if (!submitComponent.IsDirty)
            {
                continue;
            }
            
            if (!includeEIds.Contains(component.EntityId))
            {
                continue;
            }
            
            if(submitComponent.Owner != PlayerId.Invalid && player != PlayerId.Invalid && submitComponent.Owner != player )
            {
                continue;
            }
            
            componentNotifyDataPack.AddSubmitMessage(submitComponent.Id, submitComponent.TakeAllSubmitMessages());
        }
        
        return componentNotifyDataPack;
    }

    public void ReadChangedFromSubmitDataPack(IComponentSerializer serializer, ComponentSubmitMessageDataPack dataPack)
    {
        foreach (var component in Components)
        {
            var submitComponent = (ISubmitComponent)component;
            if (dataPack.SubmitMessages.TryGetValue(component.Id, out var notifyMessages))
            {
                submitComponent.ReceiveSubmitMessages(notifyMessages);
            }
        }
    }
}