using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using SEServer.Core;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData;
using SEServer.GameData.Component;
using SEServer.GameData.Data;
using Phy2DWorld = nkast.Aether.Physics2D.Dynamics.World;
using World = SEServer.Data.World;

namespace SEServer.Game.System;

/// <summary>
/// 物理运算系统
/// </summary>
[Priority(10)]
public class PhysicsSystem : ISystem
{
    public World World { get; set; } = null!;
    
    public void Init()
    {
        var physicsSingletonComponent = World.EntityManager.GetSingleton<PhysicsSingletonComponent>();

        var phy2DWorld = new Phy2DWorld(Vector2.Zero);
        physicsSingletonComponent.Phy2DWorld = phy2DWorld;

        var iterations = new SolverIterations();
        iterations.PositionIterations = 3;
        iterations.VelocityIterations = 8;
        physicsSingletonComponent.iterations = iterations;
        
        physicsSingletonComponent.ContactListener = new ContactListener(physicsSingletonComponent);
    }

    public void Update()
    {
        var collection = World.EntityManager.GetComponentDataCollection<RigidbodyComponent, TransformComponent>();
        var physicsSingle = World.EntityManager.GetSingleton<PhysicsSingletonComponent>();
        // 扫描物理组件变动
        ScanPhysicsData(physicsSingle, collection);
        // 更新物理世界
        StepWorld(physicsSingle);
        // 更新物理组件
        UpdateTransform(physicsSingle, collection);
    }

    private HashSet<EId> _deleteList = new();
    private void ScanPhysicsData(PhysicsSingletonComponent physicsSingle,
        ComponentDataCollection<RigidbodyComponent, TransformComponent> collection)
    {
        _deleteList.Clear();
        var physicsDataDic = physicsSingle.PhysicsDataDic;
        foreach (var eId in physicsDataDic.Keys)
        {
            _deleteList.Add(eId);
        }
        
        foreach (var tuple in collection)
        {
            var rigidbodyComponent = tuple.Item1;
            var transformComponent = tuple.Item2;
            var entityId = rigidbodyComponent.EntityId;
            if (_deleteList.Contains(entityId))
            {
                // 更新
                var physicsData = physicsDataDic[entityId];
                if(World.EntityManager.IsChanged(rigidbodyComponent))
                {
                    physicsData.UpdateBody(rigidbodyComponent);
                }
                UpdatePhysicsBindShape(physicsData);
                _deleteList.Remove(entityId);
            }
            else
            {
                // 新增
                var physicsData = CreatePhysicsData(physicsSingle, rigidbodyComponent);
                physicsData.InitBody(rigidbodyComponent);
                physicsData.SetPosition(transformComponent.Position);
                UpdatePhysicsBindShape(physicsData);
                physicsDataDic.Add(entityId, physicsData);
            }
        }

        // 删除
        foreach (var eId in _deleteList)
        {
            var physicsData = physicsDataDic[eId];
            RemovePhysicsData(physicsSingle, physicsData);
            physicsDataDic.Remove(eId);
        }
    }
    
    private void StepWorld(PhysicsSingletonComponent physicsSingle)
    {
        var config = (ServerWorldConfig)World.ServerContainer.Get<IWorldConfig>();
        var iterations = physicsSingle.iterations;
        iterations.PositionIterations = config.PositionIterations;
        iterations.VelocityIterations = config.VelocityIterations;
        physicsSingle.iterations = iterations;

        var timeInStep = 1f / config.FramePerSecond;
        
        physicsSingle.Phy2DWorld.Step(timeInStep, ref iterations);
        physicsSingle.Phy2DWorld.ClearForces();
    }
    
    private void UpdateTransform(PhysicsSingletonComponent physicsSingletonComponent,
        ComponentDataCollection<RigidbodyComponent, TransformComponent> collection)
    {
        var physicsDataDic = physicsSingletonComponent.PhysicsDataDic;
        foreach (var valueTuple in collection)
        {
            var rigidbodyComponent = valueTuple.Item1;
            var transformComponent = valueTuple.Item2;
            var physicsData = physicsDataDic[transformComponent.EntityId];
            physicsData.UpdateInfo(World, rigidbodyComponent, transformComponent);
        }
    }

    private HashSet<int> _deleteShapeList = new();
    /// <summary>
    /// 更新物理绑定形状
    /// </summary>
    /// <param name="physicsData"></param>
    private void UpdatePhysicsBindShape(PhysicsData physicsData)
    {
        var entity = World.EntityManager.GetEntity(physicsData.BindEntityId);
        var shapeComponent = World.EntityManager.GetComponent<ShapeComponent>(entity);
        if(shapeComponent == null)
            return;
        
        if (!World.EntityManager.IsChanged(shapeComponent))
        {
            return;
        }
        
        foreach (var pair in physicsData.Shapes)
        {
            _deleteShapeList.Add(pair.Key);
        }

        foreach (var shape in shapeComponent.Shapes)
        {
            if (_deleteShapeList.Contains(shape.Id))
            {
                // 更新
                var physicsShape = physicsData.Shapes[shape.Id];
                if(shape.IsChanged)
                {
                    UpdateShape(shape ,physicsShape.Shape);
                }
                _deleteShapeList.Remove(shape.Id);
            }
            else
            {
                // 新增
                var fixture = CreatePhysicsBindShape(physicsData.BindBody, shape);
                physicsData.Shapes.Add(shape.Id, fixture);
            }
        }
        
        // 删除
        foreach (var shapeId in _deleteShapeList)
        {
            var physicsShape = physicsData.Shapes[shapeId];
            physicsData.BindBody.Remove(physicsShape);
            physicsData.Shapes.Remove(shapeId);
        }
    }

    private Fixture CreatePhysicsBindShape(Body body,ShapeData shape)
    {
        Fixture fixture;
        
        switch (shape)
        {
            case CircleShapeData circle:
                fixture = body.CreateFixture(new CircleShape(circle.Radius, 1f));
                break;
            default:
                World.ServerContainer.Get<ILogger>().LogError($"未知的Shape类型:{shape.GetType()}");
                throw new Exception($"未知的Shape类型:{shape.GetType()}");
        }
        
        return fixture;
    }

    private void UpdateShape(ShapeData shapeData, Shape physicsShape)
    {
        switch (shapeData)
        {
            case CircleShapeData circleShapeData:
                var circleShape = (CircleShape) physicsShape;
                circleShape.Radius = circleShapeData.Radius;
                break;
            default:
                World.ServerContainer.Get<ILogger>().LogError($"未知的Shape类型:{shapeData.GetType()}");
                throw new Exception($"未知的Shape类型:{shapeData.GetType()}");
        }
    }

    private PhysicsData CreatePhysicsData(PhysicsSingletonComponent physicsSingle,RigidbodyComponent rigidbodyComponent)
    {
        var entityId = rigidbodyComponent.EntityId;
        var physicsData = new PhysicsData();
        physicsData.PBodyType = rigidbodyComponent.BodyType;
        physicsData.BindEntityId = entityId;
        
        CreateRigidbody(physicsSingle, physicsData);
        return physicsData;
    }

    private void CreateRigidbody(PhysicsSingletonComponent physicsSingle,PhysicsData physicsData)
    {
        var rigidbody = new Body();
        switch (physicsData.PBodyType)
        {
            case PhysicsBodyType.Static:
                rigidbody.BodyType = BodyType.Static;
                break;
            case PhysicsBodyType.Kinematic:
                rigidbody.BodyType = BodyType.Kinematic;
                break;
            case PhysicsBodyType.Dynamic:
                rigidbody.BodyType = BodyType.Dynamic;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        physicsData.BindBody = rigidbody;
        rigidbody.Tag = physicsData;
        physicsSingle.Phy2DWorld.Add(rigidbody);
    }
    
    private void RemovePhysicsData(PhysicsSingletonComponent physicsSingle,PhysicsData physicsData)
    {
        physicsSingle.Phy2DWorld.Remove(physicsData.BindBody);
    }
}