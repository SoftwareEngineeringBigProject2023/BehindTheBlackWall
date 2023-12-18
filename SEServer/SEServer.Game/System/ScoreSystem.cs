using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData.Component;

namespace SEServer.Game.System;

[Priority(-20)]
public class ScoreSystem : ISystem
{
    public World World { get; set; }
    public void Init()
    {
        World.EntityManager.GetSingleton<ScoreBoardGlobalViewComponent>();
    }

    public void Update()
    {
        var dataCollection = World.EntityManager.GetComponentDataCollection<PropertyComponent, ScoreViewComponent>();
        var scoreViewGlobalComponent = World.EntityManager.GetSingleton<ScoreBoardGlobalViewComponent>();
        var scoreDataGlobalComponent = World.EntityManager.GetSingleton<ScoreBoardGlobalDataComponent>();

        var playerIdHash = 0;
        
        var scoreArray = new List<ScoreData>();
        var isChanged = false;
        foreach (var valueTuple in dataCollection)
        {
            var (propertyComponent, scoreViewComponent) = valueTuple;
            var scoreData = new ScoreData
            {
                BindEntityId = propertyComponent.EntityId,
                Name = propertyComponent.Name,
                Score = scoreViewComponent.Score
            };
            scoreArray.Add(scoreData);

            if (World.EntityManager.Components.IsChanged(scoreViewComponent))
            {
                isChanged = true;
            }

            playerIdHash += propertyComponent.EntityId.Id;
        }

        if (playerIdHash != scoreDataGlobalComponent.PlayerIdHash)
        {
            isChanged = true;
            scoreDataGlobalComponent.PlayerIdHash = playerIdHash;
        }

        if (isChanged)
        {
            // 只选取前10个
            scoreViewGlobalComponent.ScoreData = scoreArray.OrderByDescending(x => x.Score).Take(10).ToList();
            World.EntityManager.MarkAsDirty(scoreViewGlobalComponent);
        }
    }
}