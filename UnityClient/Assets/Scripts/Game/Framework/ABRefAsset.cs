using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Game.Framework
{
    public class ABRefAsset
    {
        public string ABPath { get; set; }
        public AssetBundle RefAssetBundle { get; set; }

        public ABRefAsset(string abPath)
        {
            ABPath = abPath;
        }
        
        public async UniTask LoadAssetBundleAsync()
        {
            UnloadAssetBundle();
            var request = await UnityWebRequestAssetBundle.GetAssetBundle(ABPath).SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Load AssetBundle {ABPath} error: {request.error}");
                return;
            }
            
            RefAssetBundle = DownloadHandlerAssetBundle.GetContent(request);
        }
    
        public void UnloadAssetBundle()
        {
            if (RefAssetBundle != null)
            {
                RefAssetBundle.Unload(false);
                RefAssetBundle = null;
            }
        }

        public bool HasAsset(string resPath)
        {
            return RefAssetBundle.Contains(resPath);
        }

        public Object Load(string resPath, Type loadType)
        {
            var asset = RefAssetBundle.LoadAsset(resPath, loadType);
            return asset;
        }

        public async UniTask<Object> LoadAsync(string resPath, Type loadType)
        {
            var request = RefAssetBundle.LoadAssetAsync(resPath, loadType);
            await request;
            return request.asset;
        }
    }
}