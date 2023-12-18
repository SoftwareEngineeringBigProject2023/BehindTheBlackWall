using System.Collections.Generic;
using Newtonsoft.Json;
using SEServer.Data.Interface;

namespace SEServer.GameData.CTData;

public class BulletCTData : IConfigData
{
    [JsonProperty]
    public string PrimaryKey { get; set; }
    
    [JsonProperty]
    public string DisplayName { get; set; }
    
    /// <summary>
    /// 类型：0=直线，1=锁定
    /// </summary>
    [JsonProperty]
    public int Type { get; set; }
    
    [JsonProperty]
    public int GraphId { get; set; }

    /// <summary>
    /// 子弹属性集
    ///     当前属性：
    /// </summary>
    public Dictionary<string, int>? Properties { get; set; } = new();
}