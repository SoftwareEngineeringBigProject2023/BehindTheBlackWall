using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Framework
{
    public class ABResLoader : IResLoader
    {
        /// <summary>
        /// ab引用列表，优先级高的在前面
        /// </summary>
        public List<ABRefAsset> abRefAssets = new List<ABRefAsset>();
    
        public void AddABAsset(ABRefAsset abAsset)
        {
            abRefAssets.Add(abAsset);
        }
        
        public void AddABAssetToFirst(ABRefAsset abAsset)
        {
            abRefAssets.Insert(0, abAsset);
        }
    
        public void Reset()
        {
            foreach (var abRefAsset in abRefAssets)
            {
                abRefAsset.UnloadAssetBundle();
            }
            abRefAssets.Clear();
        }

        public Object Load(string resPath, Type loadType)
        {
            foreach (var abRef in abRefAssets)
            {
                if(abRef.HasAsset(resPath))
                {
                    return abRef.Load(resPath, loadType);
                }
            }

            return null;
        }

        public async UniTask<Object> LoadAsync(string resPath, Type loadType)
        {
            foreach (var abRef in abRefAssets)
            {
                if(abRef.HasAsset(resPath))
                {
                    return await abRef.LoadAsync(resPath, loadType);
                }
            }

            return null;
        }

        public byte[] LoadBytes(string resPath)
        {
            foreach (var abRef in abRefAssets)
            {
                if(abRef.HasAsset(resPath))
                {
                    var asset = abRef.Load(resPath, typeof(TextAsset));
                    if (asset is TextAsset textAsset)
                    {
                        return textAsset.bytes;
                    }
                }
            }

            return null;
        }

        public async UniTask<byte[]> LoadBytesAsync(string resPath)
        {
            foreach (var abRef in abRefAssets)
            {
                if(abRef.HasAsset(resPath))
                {
                    var asset = await abRef.LoadAsync(resPath, typeof(TextAsset));
                    if (asset is TextAsset textAsset)
                    {
                        return textAsset.bytes;
                    }
                }
            }

            return null;
        }
        
        public bool HasAsset(string resPath)
        {
            foreach (var abRef in abRefAssets)
            {
                if(abRef.HasAsset(resPath))
                {
                    return true;
                }
            }

            return false;
        }
        
        public IEnumerable<string> LoadAllAssetsName(string path, string extension, bool includeSubFolders)
        {
            foreach (var abRef in abRefAssets)
            {
                foreach (var name in abRef.RefAssetBundle.GetAllAssetNames())
                {
                    if (!name.StartsWith(path, StringComparison.OrdinalIgnoreCase)) 
                        continue;
                    
                    if (!name.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (!includeSubFolders && name.LastIndexOf('/') != path.Length)
                        continue;
                    
                    yield return name;
                }
            }
        }

        public string LoaderCode => "AssetBundle";
    }
}