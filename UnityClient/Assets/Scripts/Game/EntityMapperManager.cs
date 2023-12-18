using System;
using System.Collections.Generic;
using System.Linq;
using Game.Controller;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game
{
    public class EntityMapper
    {
        public EId EntityId { get; set; }
        public List<ControllerMapper> Controllers { get; set; } = new();
        public HashSet<Type> HavingComponent { get; set; } = new();
    }
        
    public class ControllerMapper
    {
        public EntityMapper EntityMapper { get; set; } = null!;
        public CId ComponentId { get; set; }
        public BaseController Controller { get; set; }
        public Type ControllerType { get; set; }
    }
    
    public class EntityMapperManager
    {
        public Transform EntityRoot { get; set; }
        public ClientWorld World { get; set; } = null!;
        
        public Dictionary<Type , List<BaseControllerBuilder>> ControllerBuilders { get; set; } = new();
        /// <summary>
        /// 实体控制器映射
        /// </summary>
        public List<EntityMapper> EntityMappers { get; set; } = new();
        /// <summary>
        /// 单例控制器
        /// </summary>
        public List<BaseSingletonController> SingletonControllers { get; set; } = new();

        public void Init(ClientWorld world)
        {
            World = world;
            EntityRoot = new GameObject("EntityRoot").transform;
        }
        
        public void RegisterControllerBuilder(BaseControllerBuilder builder)
        {
            if (!ControllerBuilders.TryGetValue(builder.BindType, out var builders))
            {
                builders = new List<BaseControllerBuilder>();
                ControllerBuilders.Add(builder.BindType, builders);
            }
            builders.Add(builder);
        }

        private HashSet<EId> DeleteEntities { get; } = new();
        private HashSet<ControllerMapper> DeleteComponents { get; } = new();

        public void UpdateEntities()
        {
            DeleteEntities.Clear();

            var entityManager = World.EntityManager;

            for (var index = 0; index < EntityMappers.Count; index++)
            {
                DeleteEntities.Add(EntityMappers[index].EntityId);
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
                    var mapper = CreateEntityMapper(entity);
                    EntityMappers.Add(mapper);
                }
            }
            
            // 删除实体
            foreach (var entityId in DeleteEntities)
            {
                var index = EntityMappers.FindIndex(mapper => mapper.EntityId == entityId);
                if (index != -1)
                {
                    var entityMapper = EntityMappers[index];
                    foreach (var controller in entityMapper.Controllers)
                    {
                        controller.Controller.Destroy();
                    }
                    EntityMappers.RemoveAt(index);
                }
            }
            
            // 组件的更新
            foreach (var entityMapper in EntityMappers)
            {
                var entityId = entityMapper.EntityId;
                
                DeleteComponents.Clear();
                foreach (var pair in ControllerBuilders)
                {
                    var componentType = pair.Key;
                    var mapperList = entityMapper.Controllers.Where(mapper => mapper.ControllerType == componentType);
                    var component = entityManager.Components.GetComponent(entityId, componentType);
                    if (component == null)
                    {
                        foreach (var componentMapper in mapperList)
                        {
                            DeleteComponents.Add(componentMapper);
                        }
                    }
                    else
                    {
                        if (entityMapper.HavingComponent.Contains(componentType))
                            continue;
                        
                        if (TryCreateControllerForComponent(entityMapper, component, out var componentMapper))
                        {
                            entityMapper.Controllers.AddRange(componentMapper);
                            entityMapper.HavingComponent.Add(componentType);
                        }
                    }
                }

                // 删除组件
                foreach (var componentMapper in DeleteComponents)
                {
                    try
                    {
                        componentMapper.Controller.Destroy();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                    entityMapper.Controllers.Remove(componentMapper);
                    entityMapper.HavingComponent.Remove(componentMapper.ControllerType);
                }
            }
        }
        
        public void UpdateControllers()
        {
            if(World == null)
                return;
            
            foreach (var controller in SingletonControllers)
            {
                controller.Update();
            }
            
            foreach (var entityMapper in EntityMappers)
            {
                foreach (var componentMapper in entityMapper.Controllers)
                {
                    componentMapper.Controller.Update();
                }
            }
        }

        private EntityMapper CreateEntityMapper(Entity entity)
        {
            Debug.Log($"Create Entity {entity.Id.Id}");
            var mapper = new EntityMapper()
            {
                EntityId = entity.Id,
            };

            foreach (var pair in ControllerBuilders)
            {
                if(World.EntityManager.TryGetComponent(entity, pair.Key, out var component))
                {
                    if (TryCreateControllerForComponent(mapper, component, out var componentMapper))
                    {
                        mapper.Controllers.AddRange(componentMapper);
                        mapper.HavingComponent.Add(pair.Key);
                    }
                }
            }
            
            return mapper;
        }

        private bool TryCreateControllerForComponent(EntityMapper entityMapper, IComponent component, out ControllerMapper[] mappers)
        {
            Debug.Log($"Create Component Type {component.GetType().Name} {component.Id.Id}");
            
            if (ControllerBuilders.TryGetValue(component.GetType(), out var builders))
            {
                var hasController = false;
                mappers = new ControllerMapper[builders.Count];

                for (var index = 0; index < builders.Count; index++)
                {
                    var builder = builders[index];
                    var controller = builder.BuildController();
                    var mapper = new ControllerMapper()
                    {
                        EntityMapper = entityMapper,
                        ComponentId = component.Id,
                        Controller = controller,
                        ControllerType = builder.BindType
                    };
                    controller.ControllerMapper = mapper;
                    controller.MapperManager = this;
                    
                    controller.Init();

                    hasController = true;
                    
                    mappers[index] = mapper;
                }

                return hasController;
            }
            
            mappers = default;
            return false;
        }

        public T GetEComponent<T>(ControllerMapper controllerMapper) where T : IComponent, new()
        {
            return World.EntityManager.Components.GetComponent<T>(controllerMapper.EntityMapper.EntityId);
        }
        
        public T GetEController<T>(ControllerMapper controllerMapper) where T : BaseController
        {
            var mapper = EntityMappers.FirstOrDefault(mapper => mapper.EntityId == controllerMapper.EntityMapper.EntityId);
            if (mapper != null)
            {
                foreach (var otherComponentMapper in mapper.Controllers)
                {
                    if (otherComponentMapper.Controller is T controller)
                    {
                        return controller;
                    }
                }
            }

            return default;
        }
        
        public T GetEController<T>(EId eId) where T : BaseController
        {
            var mapper = EntityMappers.FirstOrDefault(mapper => mapper.EntityId == eId);
            if (mapper != null)
            {
                foreach (var otherComponentMapper in mapper.Controllers)
                {
                    if (otherComponentMapper.Controller is T controller)
                    {
                        return controller;
                    }
                }
            }

            return default;
        }

        public void AddSingletonController(BaseSingletonController singletonController)
        {
            singletonController.MapperManager = this;
            SingletonControllers.Add(singletonController);
        }
    }
}