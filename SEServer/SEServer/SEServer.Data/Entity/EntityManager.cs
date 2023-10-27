using System.Collections.Generic;
using System.Linq;

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
    
    public T AddComponent<T>(Entity entity) where T : struct, IComponent
    {
        entity.IsDirty = true;
        return Components.AddComponent<T>(entity.Id);
    }
    
    public T GetComponent<T>(Entity entity) where T : struct, IComponent
    {
        return Components.GetComponent<T>(entity.Id);
    }
    
    public void RemoveComponent<T>(Entity entity) where T : struct, IComponent
    {
        entity.IsDirty = true;
        Components.MarkAsToBeDelete<T>(entity.Id);
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
}