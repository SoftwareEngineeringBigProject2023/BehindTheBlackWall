using System.Collections.Generic;
using SEServer.Data;

namespace SEServer.GameData.Data;

public class GameMapData
{
    /// <summary>
    /// 碰撞体
    /// </summary>
    public List<ColliderData> Colliders { get; set; } = new List<ColliderData>();

    /// <summary>
    /// 背景图
    /// </summary>
    public List<ImageData> Backgrounds { get; set; } = new List<ImageData>();
    
    /// <summary>
    /// 装饰物
    /// </summary>
    public List<ImageData> Decorations { get; set; } = new List<ImageData>();

}

/// <summary>
/// 碰撞体数据
/// </summary>
public class ColliderData
{
    public ColliderType Type { get; set; }
    public SVector2 Position { get; set; }
    public List<SVector2> Vertices { get; set; } = new List<SVector2>();
    public float Radius { get; set; }
}

public class ImageData
{
    public string Path { get; set; } = string.Empty;
    public SVector2 Position { get; set; }
    public float Rotation { get; set; }
    public SVector2 Size { get; set; }
}

public enum ColliderType
{
    Circle,
    Polygon
}