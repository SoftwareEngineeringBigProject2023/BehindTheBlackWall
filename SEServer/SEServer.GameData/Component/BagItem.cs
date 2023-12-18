using System.Collections.Generic;
using SEServer.Data.Interface;

namespace SEServer.GameData.Component;

/// <summary>
/// 背包物品基类
/// </summary>
public class BagItem
{
    /// <summary>
    /// 绑定ID
    /// </summary>
    public string BindId { get; set; } = null!;

    /// <summary>
    /// 0 - Gun 枪
    /// </summary>
    public BagItemType BindType { get; set; } = BagItemType.None;
    public int Count { get; set; }
    public Dictionary<string, int> RtProperties { get; set; } = new();
}

public enum BagItemType
{
    None = 0,
    Weapon = 1,
}