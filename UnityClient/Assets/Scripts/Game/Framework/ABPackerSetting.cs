using System;
using UnityEngine.Serialization;

namespace Game.Framework
{
    [Serializable]
    public class ABPackerSetting
    {
        /// <summary>
        /// AB包名
        /// </summary>
        public string bundleName;
        
        /// <summary>
        /// 文件目录
        /// </summary>
        public string assetDir;
        
        /// <summary>
        /// 映射路径
        /// </summary>
        public string mapPath = "Assets/";
    }
}