using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.Game.Component;
using SEServer.GameData;
using SEServer.GameData.Component;
using SEServer.GameData.CTData;
using SEServer.GameData.Data;

namespace SEServer.Game.System;

[Priority(100)]
public class MapSystem : ISystem
{
    public World World { get; set; }
    public void Init()
    {
        var mapData = World.EntityManager.GetSingleton<MapDataSingletonComponent>();

        var mapId = World.WorldSetting["MapId"];
        var mapCTData = World.ServerContainer.Get<IConfigTable>().Get<GameMapCTData>(mapId);
        
        if(mapCTData == null)
            throw new Exception($"Map {mapId} not found");
        
        LoadMap(mapCTData, mapData);
        
        var mapInfo = World.EntityManager.GetSingleton<MapInfoGlobalComponent>();
        mapInfo.MapId = mapId;
    }

    public void Update()
    {
        
    }
    
    private void LoadMap(GameMapCTData mapCtData, MapDataSingletonComponent mapData)
    {
        mapData.MapCTData = mapCtData;
        
        var entity = World.EntityManager.GetEntity(mapData.EntityId);
        var rigidbodyComponent = World.EntityManager.GetOrAddComponent<RigidbodyComponent>(entity);
        rigidbodyComponent.BodyType = PhysicsBodyType.Static;
        rigidbodyComponent.Tag = PhysicsTag.WALL;

        var transformComponent = World.EntityManager.GetOrAddComponent<TransformComponent>(entity);
        transformComponent.Position = SVector2.Zero;
        var shapeComponent = World.EntityManager.GetOrAddComponent<ShapeComponent>(entity);
        var mapSize = mapCtData.MapSize;

        var chainShapeData = new ChainShapeData(new []
        {
            new SVector2(mapSize.X / 2f, mapSize.Y / 2f),
            new SVector2(mapSize.X / 2f, -mapSize.Y / 2f),
            new SVector2(-mapSize.X / 2f, -mapSize.Y / 2f),
            new SVector2(-mapSize.X / 2f, mapSize.Y / 2f),
            new SVector2(mapSize.X / 2f, mapSize.Y / 2f),
        });
        shapeComponent.Shapes.Add(chainShapeData);
        
        foreach (var collider in mapCtData.Colliders)
        {
            switch (collider)
            {
                case ColliderDataRect rect:
                {
                    var shapeData = new RectangleShapeData(rect.Size.X, rect.Size.Y);
                    shapeData.Position = rect.Position;
                    shapeData.Rotation = rect.Rotation;
                    shapeComponent.Shapes.Add(shapeData);
                    break;
                }
                case ColliderDataCircle circle:
                {
                    var shapeData = new CircleShapeData(circle.Radius);
                    shapeData.Position = circle.Position;
                    shapeData.Rotation = circle.Rotation;
                    shapeComponent.Shapes.Add(shapeData);
                    break;
                }
            }
        }
    }
}