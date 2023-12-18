#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Framework
{
    public class ADBResLoader : IResLoader
    {
        public void Reset()
        {
            
        }

        public Object Load(string resPath, Type loadType)
        {
            return AssetDatabase.LoadAssetAtPath(resPath, loadType);
        }

        public UniTask<Object> LoadAsync(string resPath, Type loadType)
        {
            return UniTask.FromResult(Load(resPath, loadType));
        }

        public byte[] LoadBytes(string resPath)
        {
            return AssetDatabase.LoadAssetAtPath<TextAsset>(resPath).bytes;
        }

        public UniTask<byte[]> LoadBytesAsync(string resPath)
        {
            return UniTask.FromResult(LoadBytes(resPath));
        }
        
        public bool HasAsset(string resPath)
        {
            return AssetDatabase.LoadAssetAtPath(resPath, typeof(Object)) != null;
        }
        
        public IEnumerable<string> LoadAllAssetsName(string path, string extension, bool includeSubFolders)
        {
            var dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                yield break;
            }
            
            var files = Directory.GetFiles(path, "*.*", 
                includeSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                if (!file.EndsWith(extension))
                {
                    continue;
                }
                
                var filePath = file.Replace("\\", "/");
                var assetPath = filePath.Substring(filePath.IndexOf("Assets/", StringComparison.Ordinal));
                yield return assetPath;
            }
        }

        public string LoaderCode => "AssetDatabase";

    }
}
#endif