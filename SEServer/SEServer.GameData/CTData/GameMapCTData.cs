using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData.CTData;

public class GameMapCTData : IConfigData, IConfigDataInit
{
    [JsonProperty]
    public string PrimaryKey { get; set; }
    
    [JsonProperty]
    public string DisplayName { get; set; }
    
    [JsonProperty]
    public SVector2 MapSize { get; set; }
    
    /// <summary>
    /// 碰撞体
    /// </summary>
    [JsonProperty]
    public List<ColliderDataBase> Colliders { get; set; } = new List<ColliderDataBase>();

    /// <summary>
    /// 背景图
    /// </summary>
    [JsonProperty]
    public List<ImageData> Backgrounds { get; set; } = new List<ImageData>();
    
    /// <summary>
    /// 装饰物
    /// </summary>
    [JsonProperty]
    public List<ImageData> Decorations { get; set; } = new List<ImageData>();
    
    /// <summary>
    /// 特殊点设置
    /// </summary>
    [JsonProperty]
    public List<SpecialPointData> SpecialPoints { get; set; } = new List<SpecialPointData>();

    [JsonIgnore]
    public List<SpecialPointData> SpawnPoints { get; set; } = new List<SpecialPointData>();

    [JsonIgnore]
    public List<SpecialPointData> DataSpawnPoints { get; set; } = new List<SpecialPointData>();

    public void OnInit()
    {
        foreach (var specialPoint in SpecialPoints)
        {
            if (specialPoint.Type == 1)
            {
                SpawnPoints.Add(specialPoint);
            }
            else if (specialPoint.Type == 2)
            {
                DataSpawnPoints.Add(specialPoint);
            }
        }
    }
}

/// <summary>
/// 碰撞体数据
/// </summary>
public class ColliderDataBase
{
    [JsonProperty]
    public SVector2 Position { get; set; }
    
    [JsonProperty]
    public float Rotation { get; set; }
}

public class ColliderDataCircle : ColliderDataBase
{
    [JsonProperty]
    public float Radius { get; set; }
}

public class ColliderDataPolygon : ColliderDataBase
{
    [JsonProperty]
    public List<SVector2> Points { get; set; } = new List<SVector2>();
}

public class ColliderDataRect : ColliderDataBase
{
    [JsonProperty]
    public SVector2 Size { get; set; }
}

public class ImageData
{
    [JsonProperty]
    public string Path { get; set; } = string.Empty;
    
    [JsonProperty]
    public SVector2 Position { get; set; }
    
    [JsonProperty]
    public int ZIndex { get; set; }
    
    [JsonProperty]
    public float Rotation { get; set; }
    
    [JsonProperty]
    public SVector2 Size { get; set; }
    
    [JsonProperty]
    public float[] Color { get; set; } = Array.Empty<float>();
}

public class SpecialPointData
{
    [JsonProperty]
    public string Id { get; set; } = string.Empty;
    
    [JsonProperty]
    public SVector2 Position { get; set; }
    
    /// <summary>
    /// 1 - 出生点
    /// </summary>
    [JsonProperty]
    public int Type { get; set; }
}