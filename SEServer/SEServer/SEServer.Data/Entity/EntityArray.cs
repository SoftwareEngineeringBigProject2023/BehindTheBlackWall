using System.Collections;
using System.Collections.Generic;

namespace SEServer.Data;

public class EntityArray
{
    private int IdAutoIncrement { get; set; } = 1;
    public List<Entity> Entities { get; } = new();
    public Dictionary<EId, int> EntityToIndex { get; } = new();
    public HashSet<EId> DeleteEntities { get; } = new();
    public World World { get; set; }
    public HashSet<EId> CreateEntities { get; set; } = new();

    private EId CreateId()
    {
        EId id = new EId()
        {
            Id = IdAutoIncrement++
        };
        return id;
    }
    
    public Entity CreateEntity()
    {
        var id = CreateId();
        return CreateEntity(id);
    }
    
    public Entity CreateEntity(EId eId)
    {
        var entity = new Entity {Id = eId};
        entity.IsDirty = true;
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
        DeleteEntities.Add(eId);
    }
    
    public void RemoveMarkEntities()
    {
        foreach (var eId in DeleteEntities)
        {
            var index = EntityToIndex[eId];
            EntityToIndex.Remove(eId);
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

    public void ClearDirty()
    {
        foreach (var entity in Entities)
        {
            entity.IsDirty = false;
        }
        
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
    }

    public void ReadFromSnapshot(Snapshot snapshot)
    {
        Entities.Clear();
        EntityToIndex.Clear();
        foreach (var entity in snapshot.Entities)
        {
            Entities.Add(entity);
        }
        RebuildIndex();
        ClearDirty();
    }

    public List<EId> GetChangedEntityIds()
    {
        var list = new List<EId>();
        foreach (var entity in Entities)
        {
            if (entity.IsDirty)
            {
                list.Add(entity.Id);
            }
        }
        return list;
    }
}