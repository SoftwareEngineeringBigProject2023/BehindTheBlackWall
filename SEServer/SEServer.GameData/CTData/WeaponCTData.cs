using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.GameData.CTData;

[Serializable]
public class WeaponCTData : IConfigData
{
    /// <summary>
    /// ID
    /// </summary>
    [JsonProperty]
    public string PrimaryKey { get; set; } = null!;
    
    /// <summary>
    /// 名称
    /// </summary>
    [JsonProperty]
    public string DisplayName { get; set; } = null!;
    
    /// <summary>
    /// 描述
    /// </summary>
    [JsonProperty]
    public string Description { get; set; } = null!;
    
    /// <summary>
    /// 射击间隔
    /// </summary>
    [JsonProperty]
    public float ShootInterval { get; set; } = 1f;
    
    /// <summary>
    /// 弹匣容量
    /// </summary>
    [JsonProperty]
    public int BulletCapacity { get; set; } = 10;
    
    /// <summary>
    /// 装弹时间
    /// </summary>
    [JsonProperty]
    public float ReloadTime { get; set; } = 2.5f;
    
    /// <summary>
    /// 基础伤害
    /// </summary>
    [JsonProperty]
    public float BaseDamage { get; set; } = 1f;
    
    /// <summary>
    /// 飞行速度
    /// </summary>
    [JsonProperty]
    public float BulletSpeed { get; set; } = 30f;
    
    /// <summary>
    /// 射程
    /// </summary>
    [JsonProperty]
    public float Range { get; set; } = 60f;

    [JsonProperty]
    public int StartFlame { get; set; } = 0;
    
    [JsonProperty]
    public int ShootSound { get; set; } = 0;

    /// <summary>
    /// 子弹数据
    /// </summary>
    [JsonProperty]
    public List<GunBulletSubData> ShootBullets { get; set; } = new List<GunBulletSubData>();

    [Serializable]
    public class GunBulletSubData
    {
        [JsonProperty]
        public string BulletId { get; set; } = null!;
        /// <summary>
        /// 旋转角度
        /// </summary>
        [JsonProperty]
        public float Angle { get; set; }
        /// <summary>
        /// 初始位置，受旋转角度影响
        /// </summary>
        [JsonProperty]
        public SVector2 Position { get; set; }
    }
}