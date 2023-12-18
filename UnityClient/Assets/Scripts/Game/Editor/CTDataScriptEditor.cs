using Game.Utils;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public static class CTDataScriptEditor
    {
        [MenuItem("工具/数据/导出地图数据到服务器", false, 500)]
        public static void CopyMapDataToServer()
        {
            var mapResPath = ConStr.MapDataRoot;
            var exportResPath = ConStr.MapDataServerPathRoot;
            
            FileUtils.CopyDirectory(mapResPath, exportResPath, "*.json");
        }
        
        [MenuItem("工具/数据/导出配置表数据到服务器", false, 500)]
        public static void CopyConfigDataToClient()
        {
            var configResPath = ConStr.ConfigDataRoot;
            var exportResPath = ConStr.ConfigDataServerPathRoot;
            
            FileUtils.CopyDirectory(configResPath, exportResPath, "*.json");
        }
    }
}