﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace SEServer.Data;

public class EntityArray
{
    private int IdAutoIncrement { get; set; } = 1;
    public List<Entity> Entities { get; } = new();
    public Dictionary<EId, int> EntityToIndex { get; } = new();
    public HashSet<EId> DeleteEntities { get; } = new();
    public World World { get; set; } = null!;
    public HashSet<EId> CreateEntities { get; set; } = new();
    private HashSet<EId> DirtyEntities { get; set; } = new();

    private Entity SingletonEntity { get; set; } = null!;

    private EId CreateId()
    {
        EId id = new EId()
        {
            Id = IdAutoIncrement++
        };
        return id;
    }

    public void Init()
    {
        // 创建单例实体
        SingletonEntity = CreateEntity();
    }
    
    public Entity CreateEntity()
    {
        var id = CreateId();
        return CreateEntity(id);
    }
    
    public Entity CreateEntity(EId eId)
    {
        var entity = new Entity {Id = eId};
        MarkAsDirty(entity);
        Entities.Add(entity);
        EntityToIndex.Add(entity.Id, Entities.Count - 1);
        CreateEntities.Add(entity.Id);
        return entity;
    }
    
    public Entity GetEntity(EId id)
    {
        if(EntityToIndex.TryGetValue(id, out var value))
            return Entities[value];
        
        return Entity.Empty;
    }

    public void MarkAsToBeDelete(EId eId)
    {
        if (!EntityToIndex.ContainsKey(eId))
            return;
        
        DeleteEntities.Add(eId);
    }
    
    private List<int> _toDeleteIndex = new List<int>();
    /// <summary>
    /// 将标记为删除的实体从数组中移除
    /// </summary>
    public void RemoveMarkEntities()
    {
        _toDeleteIndex.Clear();
        foreach (var eId in DeleteEntities)
        {
            if (!EntityToIndex.TryGetValue(eId, out var index))
                continue;
            
            _toDeleteIndex.Add(index);
        }

        _toDeleteIndex.Sort();
        for (int i = _toDeleteIndex.Count - 1; i >= 0; i--)
        {
            var index = _toDeleteIndex[i];
            Entities.RemoveAt(index);
        }

        // 重建索引
        RebuildIndex();
    }

    private void RebuildIndex()
    {
        EntityToIndex.Clear();
        for (int i = 0; i < Entities.Count; i++)
        {
            EntityToIndex.Add(Entities[i].Id, i);
        }
    }
    
    public void MarkAsDirty(Entity entity)
    {
        DirtyEntities.Add(entity.Id);
    }
    
    private bool IsDirty(Entity entity)
    {
        return DirtyEntities.Contains(entity.Id);
    }

    public void ClearDirty()
    {
        DirtyEntities.Clear();
        DeleteEntities.Clear();
        CreateEntities.Clear();
    }

    public void WriteToSnapshot(Snapshot snapshot)
    {
        foreach (var entity in Entities)
        {
            var newEntity = new Entity();
            newEntity.Id = entity.Id;
            snapshot.Entities.Add(newEntity);
        }
        snapshot.SingletonEntityId = SingletonEntity.Id;
    }

    public void ReadFromSnapshot(Snapshot snapshot)
    {
        Entities.Clear();
        EntityToIndex.Clear();
        foreach (var entity in snapshot.Entities)
        {
            Entities.Add(entity);
        }

        var singleton = Entities.Find(e => e.Id == snapshot.SingletonEntityId);
        if(singleton == null)
            throw new Exception("Singleton entity not found !");
        
        SingletonEntity = singleton;
        RebuildIndex();
        ClearDirty();
    }

    public List<EId> GetChangedEntityIds()
    {
        var list = new List<EId>();
        foreach (var entity in Entities)
        {
            if (IsDirty(entity))
            {
                list.Add(entity.Id);
            }
        }
        return list;
    }

    public Entity GetSingletonEntity()
    {
        return SingletonEntity;
    }
}