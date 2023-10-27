using System;
using System.Collections.Generic;
using System.Linq;
using SEServer.Data.Interface;

namespace SEServer.Data;

/// <summary>
/// 组件列表
/// </summary>
public class ComponentArray<T> : IComponentArray where T : IComponent, new()
{
    public World World { get; set; } = null!;
    private int IdAutoIncrement { get; set; } = 1;
    private Dictionary<EId, CId> EntityToComponents { get; } = new();
    private Dictionary<CId, EId> ComponentToEntity { get; } = new();
    private Dictionary<CId, int> ComponentToIndex { get; } = new();
    private HashSet<EId> DeleteEntities { get; } = new();
    private List<Type> Interfaces { get; } = new();
    /// <summary>
    /// 脏标记组件，被标记的组件会通过网络进行同步
    /// </summary>
    private HashSet<CId> DirtyComponents { get; } = new();
    /// <summary>
    /// 有变动的组件，与Dirty不同，变动组件标记该组件需要被System更新
    /// </summary>
    private HashSet<CId> ChangedComponents { get; } = new();

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
        component.EntityId = eId;
        AddComponent(component);

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
    
    public IComponent GetI(EId eId)
    {
        return Components[ComponentToIndex[EntityToComponents[eId]]];
    }

    public IEnumerable<IComponent> GetAllComponents()
    {
        return Components.Cast<IComponent>();
    }

    /// <summary>
    /// 将实体的组件标记为删除
    /// </summary>
    /// <param name="eId"></param>
    public void MarkAsToBeDelete(EId eId)
    {
        DeleteEntities.Add(eId);
    }

    public void MarkAsDirty(INetComponent component)
    {
        DirtyComponents.Add(component.Id);
    }

    public bool IsDirty(INetComponent component)
    {
        return DirtyComponents.Contains(component.Id);
    }
    
    public void ClearDirty()
    {
        DirtyComponents.Clear();
    }
    
    public void MarkAsChanged(IComponent component)
    {
        ChangedComponents.Add(component.Id);
    }
    
    public bool IsChanged(IComponent component)
    {
        return ChangedComponents.Contains(component.Id);
    }
    
    public void ClearChanged()
    {
        ChangedComponents.Clear();
    }
    
    /// <summary>
    /// 写入快照
    /// </summary>
    /// <param name="serializer"></param>
    /// <returns></returns>
    public ComponentArrayDataPack WriteToDataPack(IComponentSerializer serializer)
    {
        var components = new List<T>();
        components.AddRange(Components);
        var dataPack = serializer.Serialize(components);
        return dataPack;
    }

    /// <summary>
    /// 从快照中读取组件列表
    /// </summary>
    /// <param name="serializer"></param>
    /// <param name="dataPack"></param>
    public void ReadFromDataPack(IComponentSerializer serializer, ComponentArrayDataPack dataPack)
    {
        Components.Clear();
        EntityToComponents.Clear();
        ComponentToIndex.Clear();

        var components = serializer.Deserialize<T>(dataPack);
        for (var index = 0; index < components.Count; index++)
        {
            var component = components[index];
            // 如果拥有者不是自己或全体玩家，则不添加
            if (component is IC2SComponent c2SComponent && World is ClientWorld clientWorld)
            {
                if(c2SComponent.Owner != PlayerId.Invalid && c2SComponent.Owner != clientWorld.PlayerId)
                {
                    continue;
                }
            }
            
            Components.Add(component);
            MarkAsChanged(component);
        }
        
        // 重建索引
        RebuildIndex();
    }

    public ComponentArrayDataPack? WriteChangedToDataPack(IComponentSerializer serializer, List<EId> includeEIds, PlayerId player)
    {
        var components = new List<T>();
        
        foreach (var component in Components)
        {
            var netComponent = (INetComponent)component;
            if (!IsDirty(netComponent) && !includeEIds.Contains(component.EntityId))
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
            
            components.Add(component);
        }

        if (components.Count == 0)
            return null;
        
        var dataPack = serializer.Serialize(components);
        return dataPack;
    }

    public void ReadChangedFromDataPack(IComponentSerializer serializer, ComponentArrayDataPack dataPack)
    {
        var components = serializer.Deserialize<T>(dataPack);
        for (var index = 0; index < components.Count; index++)
        {
            var component = components[index];
            
            // 如果拥有者不是自己或全体玩家，则不添加
            if (component is IC2SComponent c2SComponent && World is ClientWorld clientWorld)
            {
                if(c2SComponent.Owner != PlayerId.Invalid && c2SComponent.Owner != clientWorld.PlayerId)
                {
                    continue;
                }
            }
            
            var hasComponent = ComponentToIndex.ContainsKey(component.Id);
            if (hasComponent)
            {
                Components[ComponentToIndex[component.Id]] = component;
            }
            else
            {
                AddComponent(component);
            }
            
            // 标记为已改变
            MarkAsChanged(component);
        }
    }
    
    /// <summary>
    /// 写空提交给客户端作为提交模板
    /// </summary>
    /// <param name="serializer"></param>
    /// <param name="allEntitiesChanged"></param>
    /// <returns></returns>
    public ComponentArrayDataPack? WriteEmptySubmitToDataPack(IComponentSerializer serializer, List<EId> allEntitiesChanged)
    {
        var components = new List<T>();
        
        foreach (var component in Components)
        {
            var submitComponent = (ISubmitComponent)component;
            if (!IsDirty(submitComponent) && !allEntitiesChanged.Contains(component.EntityId))
            {
                continue;
            }

            var emptySubmitComponent = (ISubmitComponent)new T();
            emptySubmitComponent.Id = component.Id;
            emptySubmitComponent.EntityId = component.EntityId;
            emptySubmitComponent.Owner = submitComponent.Owner;
            components.Add((T)emptySubmitComponent);
        }

        if (components.Count == 0)
            return null;
        
        var dataPack = serializer.Serialize(components);
        return dataPack;
    }

    private List<INotifyComponent> _tmpNotifyComList = new();
    public ComponentNotifyMessageDataPack? WriteChangedToNotifyDataPack(IComponentSerializer serializer, List<EId> includeEIds)
    {
        _tmpNotifyComList.Clear();
        foreach (var component in Components)
        {
            var notifyComponent = (INotifyComponent)component;
            if (notifyComponent.NotifyMessages.Count == 0)
            {
                continue;
            }
            
            _tmpNotifyComList.Add(notifyComponent);
        }

        if (_tmpNotifyComList.Count == 0)
        {
            return null;
        }
        
        var componentNotifyDataPack = new ComponentNotifyMessageDataPack();
        componentNotifyDataPack.TypeCode = serializer.GetCodeByType(typeof(T));
        foreach (var notifyComponent in _tmpNotifyComList)
        {
            componentNotifyDataPack.AddNotifyMessage(notifyComponent.Id, notifyComponent.TakeAllNotifyMessages());
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
                MarkAsChanged(notifyComponent);
            }
        }
    }

    List<ISubmitComponent> _tmpSubmitComList = new();
    public ComponentSubmitMessageDataPack? WriteChangedToSubmitDataPack(IComponentSerializer serializer, List<EId> includeEIds, PlayerId player)
    {
        _tmpSubmitComList.Clear();
        
        foreach (var component in Components)
        {
            var submitComponent = (ISubmitComponent)component;
            if (submitComponent.SubmitMessages.Count == 0)
            {
                continue;
            }

            if(submitComponent.Owner != PlayerId.Invalid && player != PlayerId.Invalid && submitComponent.Owner != player )
            {
                continue;
            }
            
            _tmpSubmitComList.Add(submitComponent);
        }

        if (_tmpSubmitComList.Count == 0)
        {
            return null;
        }
        
        var componentNotifyDataPack = new ComponentSubmitMessageDataPack();
        componentNotifyDataPack.TypeCode = serializer.GetCodeByType(typeof(T));
        foreach (var submitComponent in _tmpSubmitComList)
        {
            componentNotifyDataPack.AddSubmitMessage(submitComponent.Id, submitComponent.TakeAllSubmitMessages());
            submitComponent.ClearSubmitMessages();
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
                MarkAsChanged(submitComponent);
            }
        }
    }
}