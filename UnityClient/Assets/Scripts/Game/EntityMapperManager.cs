using System;
using System.Collections.Generic;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game
{
    public class EntityMapperManager
    {
        public Transform EntityRoot { get; set; }
        public ClientWorld World { get; set; } = null!;
        
        public struct EntityMapper
        {
            public EId Entity { get; set; }
            public GameObject GameObject { get; set; }
        }
        
        public struct ComponentMapper
        {
            public CId Component { get; set; }
            public EId Entity { get; set; }
            public MonoBehaviour Controller { get; set; }
        }
        
        public Dictionary<Type ,BaseControllerBuilder> ControllerBuilders { get; set; } = new();
        
        public List<EntityMapper> EntityMappers { get; set; } = new();
        public Dictionary<Type, List<ComponentMapper>> ComponentMappers { get; set; } = new();

        public void Init(ClientWorld world)
        {
            World = world;
            EntityRoot = new GameObject("EntityRoot").transform;
            
            RegisterControllerBuilder(new TransformControllerBuilder());
            RegisterControllerBuilder(new GraphControllerBuilder());

            foreach (var controllerBuilder in ControllerBuilders)
            {
                ComponentMappers.Add(controllerBuilder.Key, new List<ComponentMapper>());
            }
        }
        
        private void RegisterControllerBuilder(BaseControllerBuilder builder)
        {
            ControllerBuilders.Add(builder.BindType, builder);
        }

        private HashSet<EId> DeleteEntities { get; } = new();
        private HashSet<EId> CreateEntities { get; } = new();
        private HashSet<CId> DeleteComponents { get; } = new();
        
        public void UpdateEntities()
        {
            DeleteEntities.Clear();
            CreateEntities.Clear();

            for (var index = 0; index < EntityMappers.Count; index++)
            {
                DeleteEntities.Add(EntityMappers[index].Entity);
            }
            
            foreach (var entity in World.EntityManager.Entities.Entities)
            {
                if (DeleteEntities.Contains(entity.Id))
                {
                    // 如果依然存在，则不需要删除
                    DeleteEntities.Remove(entity.Id);
                }
                else
                {
                    // 如果不存在，则创建
                    var mapper = CreateEntityToGameObject(World ,entity);
                    EntityMappers.Add(mapper);
                    CreateEntities.Add(entity.Id);
                }
            }
            
            // 组件的更新
            foreach (var pair in ComponentMappers)
            {
                DeleteComponents.Clear();
                foreach (var mapper in pair.Value)
                {
                    if (DeleteEntities.Contains(mapper.Entity))
                    {
                        // 避免重复删除开销
                        continue;
                    }
                    if (CreateEntities.Contains(mapper.Entity))
                    {
                        // 防止删除新创建的组件
                        continue;
                    }
                    
                    DeleteComponents.Add(mapper.Component);
                }

                var array = World.EntityManager.Components.GetComponentArray(pair.Key);
                foreach (var component in array.GetAllComponents())
                {
                    DeleteComponents.Remove(component.Id);
                }
                
                foreach (var componentId in DeleteComponents)
                {
                    var index = pair.Value.FindIndex(mapper => mapper.Component == componentId);
                    if (index != -1)
                    {
                        var mapper = pair.Value[index];
                        pair.Value.RemoveAt(index);
                        Object.Destroy(mapper.Controller);
                    }
                }
            }
            
            // 删除实体
            foreach (var entityId in DeleteEntities)
            {
                var index = EntityMappers.FindIndex(mapper => mapper.Entity == entityId);
                if (index != -1)
                {
                    var mapper = EntityMappers[index];
                    EntityMappers.RemoveAt(index);
                    Object.Destroy(mapper.GameObject);
                }
            }
        }

        private EntityMapper CreateEntityToGameObject(World world,Entity entity)
        {
            Debug.Log($"Create Entity {entity.Id.Id}");
            
            var go = new GameObject();
            go.transform.SetParent(EntityRoot);
            go.name = $"Entity_{entity.Id.Id}";
            var mapper = new EntityMapper()
            {
                Entity = entity.Id,
                GameObject = go
            };

            foreach (var pair in ControllerBuilders)
            {
                if(world.EntityManager.TryGetComponent(entity, pair.Key, out var component))
                {
                    if (TryCreateComponentToController(go, component, out var componentMapper))
                    {
                        ComponentMappers[pair.Key].Add(componentMapper);
                    }
                }
            }
            
            return mapper;
        }

        private bool TryCreateComponentToController(GameObject go, IComponent component, out ComponentMapper mapper)
        {
            Debug.Log($"Create Component {component.Id.Id}");
            
            if (ControllerBuilders.TryGetValue(component.GetType(), out var builder))
            {
                var mono = builder.BuildController(go, component);
                mapper = new ComponentMapper()
                {
                    Component = component.Id,
                    Entity = component.EntityId,
                    Controller = mono
                };
                mono.ComponentMapper = mapper;
                mono.MapperManager = this;
                return true;
            }
            
            mapper = default;
            return false;
        }

        public T GetEComponent<T>(ComponentMapper componentMapper) where T : IComponent, new()
        {
            return World.EntityManager.Components.GetComponent<T>(componentMapper.Entity);
        }
    }
}