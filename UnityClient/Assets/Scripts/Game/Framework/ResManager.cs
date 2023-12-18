using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Game.Framework
{
    public class ResManager : MonoSingleton<ResManager>
    {
#if UNITY_EDITOR
        public ADBResLoader ADBResLoader { get; } = new ADBResLoader();
#endif
        public ABResLoader ABResLoader { get; } = new ABResLoader();

        public IEnumerable<IResLoader> ResLoaders
        {
            get
            {
#if UNITY_EDITOR && !AB_LOAD_TEST
                yield return ADBResLoader;
#else
                yield return ABResLoader;
#endif
            }
        }
        
        private SLogger _logger = new SLogger("ResManager");

#pragma warning disable CS1998
        public async UniTask Init()
#pragma warning restore CS1998
        {
            Reset();

#if !UNITY_EDITOR || AB_LOAD_TEST
            await LoadStreamingAssetsBundle();
#endif
        }

        private async UniTask LoadStreamingAssetsBundle()
        {
            var streamingAssetsPath = Application.streamingAssetsPath;
            var abBundleListPath = Path.Combine(streamingAssetsPath, "AssetBundleList.txt");
            
            var abBundleList = await UnityWebRequest.Get(abBundleListPath).SendWebRequest();
            if (abBundleList.result != UnityWebRequest.Result.Success)
            {
                _logger.LogError($"Load AssetBundleList.txt error: {abBundleList.error}");
                return;
            }
            
            var abBundleListText = abBundleList.downloadHandler.text;
            var abBundleNames = abBundleListText.Split('\n');
            foreach (var abBundleName in abBundleNames)
            {
                var fileName = abBundleName.Trim();
                if (string.IsNullOrEmpty(fileName))
                {
                    continue;
                }
                
                var abRefAsset = new ABRefAsset(Path.Combine(streamingAssetsPath, fileName));
                await abRefAsset.LoadAssetBundleAsync();
                ABResLoader.AddABAsset(abRefAsset);
            }
        }

        public void Reset()
        {
            // 清理资源缓存
            foreach (var resLoader in ResLoaders)
            {
                resLoader.Reset();
            }

            Resources.UnloadUnusedAssets();
        }

        #region 资源加载接口

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <param name="loadType"></param>
        /// <returns></returns>
        public async UniTask<Object> LoadAssetAsync(string path, Type loadType)
        {
            foreach (var resLoader in ResLoaders)
            {
                if (resLoader.HasAsset(path))
                {
                    return await resLoader.LoadAsync(path, loadType);
                }
            }

            return null;
        }

        /// <summary>
        /// 异步加载资源，泛型接口
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<T> LoadAssetAsync<T>(string path) where T : Object
        {
            return await LoadAssetAsync(path, typeof(T)) as T;
        }

        /// <summary>
        /// 同步加载资源，返回值表示资源是否存在且加载完毕
        /// </summary>
        /// <param name="path"></param>
        /// <param name="loadType"></param>
        /// <returns></returns>
        public Object LoadAsset(string path, Type loadType)
        {
            foreach (var resLoader in ResLoaders)
            {
                if (resLoader.HasAsset(path))
                {
                    var asset = resLoader.Load(path, loadType);
                    return asset;
                }
            }

            return null;
        }

        /// <summary>
        /// 同步加载泛型接口
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LoadAsset<T>(string path) where T : Object
        {
            return LoadAsset(path, typeof(T)) as T;
        }

        public byte[] LoadBytes(string path)
        {
            foreach (var resLoader in ResLoaders)
            {
                if (resLoader.HasAsset(path))
                {
                    return resLoader.LoadBytes(path);
                }
            }

            return null;
        }

        public async UniTask<byte[]> LoadBytesAsync(string path)
        {
            foreach (var resLoader in ResLoaders)
            {
                if (resLoader.HasAsset(path))
                {
                    return await resLoader.LoadBytesAsync(path);
                }
            }

            return null;
        }

        public void LoadScene(string path, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(path, mode);
        }

        public async UniTask LoadSceneAsync(string path, LoadSceneMode mode = LoadSceneMode.Single, Action<float> progress = null)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(path, mode);
            
            while (!asyncOperation.isDone)
            {
                progress?.Invoke(asyncOperation.progress);
                await UniTask.Yield();
            }
        }

        /// <summary>
        /// 是否存在资源
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool HaveAsset(string path)
        {
            foreach (var resLoader in ResLoaders)
            {
                if (resLoader.HasAsset(path))
                {
                    return true;
                }
            }

            return false;
        }


        #endregion
        
        public GameObject Spawn(string assetPath)
        {
            var prefab = LoadAsset<GameObject>(assetPath);
            if (prefab == null)
            {
                _logger.LogError($"SpawnPrefab failed, assetPath: {assetPath}");
                return null;
            }
            
            var go = Instantiate(prefab);
            go.name = prefab.name;
            return go;
        }

        public GameObject Spawn(string assetPath, Transform goRoot)
        {
            var prefab = LoadAsset<GameObject>(assetPath);
            if (prefab == null)
            {
                _logger.LogError($"SpawnPrefab failed, assetPath: {assetPath}");
                return null;
            }
            
            var go = Instantiate(prefab, goRoot);
            go.name = prefab.name;
            return go;
        }
        
        public async UniTask<GameObject> SpawnAsync(string assetPath)
        {
            var prefab = await LoadAssetAsync<GameObject>(assetPath);
            if (prefab == null)
            {
                _logger.LogError($"SpawnPrefab failed, assetPath: {assetPath}");
                return null;
            }
            
            var go = Instantiate(prefab);
            go.name = prefab.name;
            return go;
        }
        
        public async UniTask<GameObject> SpawnAsync(string assetPath, Transform goRoot)
        {
            var prefab = await LoadAssetAsync<GameObject>(assetPath);
            if (prefab == null)
            {
                _logger.LogError($"SpawnPrefab failed, assetPath: {assetPath}");
                return null;
            }
            
            var go = Instantiate(prefab, goRoot);
            go.name = prefab.name;
            return go;
        }

        public IEnumerable<string> LoadAllAssetsName(string path, string extension, bool includeSubFolders)
        {
            foreach (var resLoader in ResLoaders)
            {
                foreach (var assetName in resLoader.LoadAllAssetsName(path, extension, includeSubFolders))
                {
                    yield return assetName;
                }
            }
        }
        
        public IEnumerable<T> LoadAllAssets<T>(string path, string extension, bool includeSubFolders) where T : Object
        {
            foreach (var assetPath in LoadAllAssetsName(path, extension, includeSubFolders))
            {
                var asset = LoadAsset<T>(assetPath);
                if (asset != null)
                {
                    yield return asset;
                }
            }
        }
    }
}