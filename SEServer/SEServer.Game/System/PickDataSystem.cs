using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.Game.Component;
using SEServer.GameData;
using SEServer.GameData.Builder;
using SEServer.GameData.Component;
using SEServer.GameData.Data;

namespace SEServer.Game.System;

public class PickDataSystem : ISystem
{
    public World World { get; set; }
    public void Init()
    {
        var pickSpawn = World.EntityManager.GetSingleton<PickSpawnSingletonComponent>();

        var mapData = World.EntityManager.GetSingleton<MapDataSingletonComponent>();
        foreach (var specialPointData in mapData.MapCTData.DataSpawnPoints)
        {
            var pickSpawnData = new PickSpawnData()
            {
                BindSpecialPoint = specialPointData
            };
            pickSpawn.PickSpawnDataList.Add(pickSpawnData);
        }
    }

    public void Update()
    {
        var deltaTime = World.Time.DeltaTime;
        var pickSpawn = World.EntityManager.GetSingleton<PickSpawnSingletonComponent>();
        
        var pickDataCollection = World.EntityManager.GetComponentDataCollection<PickDataComponent>();
        var physicsSingleton = World.EntityManager.GetSingleton<PhysicsSingletonComponent>();
        foreach (var pickDataComponent in pickDataCollection)
        {
            if (!physicsSingleton.PhysicsDataDic.TryGetValue(pickDataComponent.EntityId, out var physicsData))
            {
                continue;
            }

            foreach (var pair in physicsData.Contacts)
            {
                var contact = pair.Value;
                if (contact.OtherPhysicsData.BindBody.Tag is not PhysicsData otherPhysicsData)
                {
                    continue;
                }

                var otherRigidbody = World.EntityManager.GetComponent<RigidbodyComponent>(otherPhysicsData.BindEntityId);
                if (otherRigidbody is not { Tag: PhysicsTag.UNIT })
                {
                    continue;
                }

                var pickSpawnData = pickSpawn.PickSpawnDataList.TryGet(pickDataComponent.BindIndex);
                if (pickSpawnData == null)
                {
                    continue;
                }
                
                pickSpawnData.HasPickData = false;
                pickSpawnData.PickDataSpawnCooldown = 10f;
                
                World.EntityManager.Entities.MarkAsToBeDelete(pickDataComponent.EntityId);

                var scoreView = World.EntityManager.GetComponent<ScoreViewComponent>(otherRigidbody.EntityId);
                if (scoreView != null)
                {
                    scoreView.Score += pickDataComponent.Score;
                    World.EntityManager.MarkAsDirty(scoreView);
                    
                    var globalNotify = World.EntityManager.GetSingleton<PlayerNotifyGlobalComponent>();
                    globalNotify.AddNotifyMessage(new UnitScoreChangedNotifyGlobalMessageBuilder()
                        .SetEntityId(otherRigidbody.EntityId.Id)
                        .SetScore(pickDataComponent.Score)
                        .Build());

                    globalNotify.AddNotifyMessage(new UnitPickupNotifyGlobalMessageBuilder()
                        .SetEntityId(otherRigidbody.EntityId.Id)
                        .Build());
                }
                
                break;
            }
        }
        
        for (var index = 0; index < pickSpawn.PickSpawnDataList.Count; index++)
        {
            var data = pickSpawn.PickSpawnDataList[index];
            if (data.HasPickData)
                continue;
            
            data.PickDataSpawnCooldown -= deltaTime;
                
            if (data.PickDataSpawnCooldown <= 0)
            {
                SpawnPickData(data, index);
            }
        }
    }

    private void SpawnPickData(PickSpawnData data, int index)
    {
        data.HasPickData = true;
        
        var pickDataEntity = World.EntityManager.CreateEntity();

        var transform = World.EntityManager.GetOrAddComponent<TransformComponent>(pickDataEntity);
        transform.Position = data.BindSpecialPoint.Position;

        var graphData = World.EntityManager.GetOrAddComponent<GraphComponent>(pickDataEntity);
        graphData.Type = GraphType.PickData;

        var pickComponent = World.EntityManager.GetOrAddComponent<PickDataComponent>(pickDataEntity);
        pickComponent.BindIndex = index;
        pickComponent.Score = 20;
        
        var rigidbodyComponent = World.EntityManager.GetOrAddComponent<RigidbodyComponent>(pickDataEntity);
        rigidbodyComponent.IsTrigger = true;

        var shapeComponent = World.EntityManager.GetOrAddComponent<ShapeComponent>(pickDataEntity);
        shapeComponent.Shapes.Add(new CircleShapeData(0.5f));
    }
}