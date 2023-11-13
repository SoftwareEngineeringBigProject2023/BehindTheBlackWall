using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Game.Framework
{
    public class ResManager : MonoSingleton<ResManager>
    {
        public Dictionary<string, ResMapper> resMappers =
            new Dictionary<string, ResMapper>(StringComparer.OrdinalIgnoreCase);

#if UNITY_EDITOR
        public ADBResLoader ADBResLoader { get; } = new ADBResLoader();
#endif
        public ABResLoader ABResLoader { get; } = new ABResLoader();

        public IEnumerable<IResLoader> ResLoaders
        {
            get
            {
#if UNITY_EDITOR
                yield return ADBResLoader;
#else
                yield return ABResLoader;
#endif
            }
        }
        
        private SLogger _logger = new SLogger("ResManager");

        public void Init()
        {
            Reset();
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

        public void CacheAssetBundleDir(string rootPath)
        {
            if (!Directory.Exists(rootPath))
            {
                return;
            }

            var abPaths = Directory.GetFiles(rootPath, "*.ab", SearchOption.AllDirectories);
            foreach (var abPath in abPaths)
            {
                AddABAsset(abPath);
            }
        }

        #region 资源加载接口

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async UniTask<Object> LoadAssetAsync(string path)
        {
            if (resMappers.TryGetValue(path, out var resMapper))
            {
                return await resMapper.LoadAsync();
            }

            foreach (var pair in resMappers)
            {
                if (pair.Key.Contains(path))
                {
                    AddResMapper(path, new ResMapper(path, pair.Value.resLoader));
                    return await pair.Value.LoadAsync();
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
            return await LoadAssetAsync(path) as T;
        }

        /// <summary>
        /// 同步加载资源，返回值表示资源是否存在且加载完毕
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Object LoadAsset(string path)
        {
            if (resMappers.TryGetValue(path, out var resMapper))
            {
                return resMapper.Load();
            }
            
            foreach (var pair in resMappers)
            {
                if (pair.Key.Contains(path))
                {
                    AddResMapper(path, new ResMapper(path, pair.Value.resLoader));
                    return pair.Value.Load();
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
            return LoadAsset(path) as T;
        }

        public byte[] LoadBytes(string path)
        {
            if (resMappers.TryGetValue(path, out var resMapper))
            {
                return resMapper.LoadBytes();
            }
            
            foreach (var pair in resMappers)
            {
                if (pair.Key.Contains(path))
                {
                    AddResMapper(path, new ResMapper(path, pair.Value.resLoader));
                    return pair.Value.LoadBytes();
                }
            }

            return null;
        }

        public async UniTask<byte[]> LoadBytesAsync(string path)
        {
            if (resMappers.TryGetValue(path, out var resMapper))
            {
                return await resMapper.LoadBytesAsync();
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
            return resMappers.ContainsKey(path);
        }

        public void AddABAsset(string path)
        {
            var abAsset = new ABRefAsset(path);
            abAsset.LoadAssetBundle();

            var allAssetsName = abAsset.RefAssetBundle.GetAllAssetNames();
            foreach (var resPath in allAssetsName)
            {
                if (!resPath.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase))
                    continue;

                AddResMapper(resPath, new ResMapper(resPath, ABResLoader));
            }

            ABResLoader.AddABAssetToFirst(abAsset);
        }

        public void AddResMapper(string path, ResMapper resMapper)
        {
            resMappers[path] = resMapper;
        }

        #endregion
    }
}