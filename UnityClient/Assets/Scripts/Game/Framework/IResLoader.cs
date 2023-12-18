using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Game.Framework
{
    public interface IResLoader
    {
        void Reset();

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <param name="resPath"></param>
        /// <param name="loadType"></param>
        /// <returns></returns>
        Object Load(string resPath, Type loadType);

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="resPath"></param>
        /// <param name="loadType"></param>
        /// <returns></returns>
        UniTask<Object> LoadAsync(string resPath, Type loadType);
    
        /// <summary>
        /// 加载字节流
        /// </summary>
        /// <param name="resPath"></param>
        /// <returns></returns>
        byte[] LoadBytes(string resPath);
    
        /// <summary>
        /// 异步加载字节流
        /// </summary>
        /// <param name="resPath"></param>
        /// <returns></returns>
        UniTask<byte[]> LoadBytesAsync(string resPath);
        
        bool HasAsset(string resPath);

        string LoaderCode { get; }
        IEnumerable<string> LoadAllAssetsName(string path, string extension, bool includeSubFolders);
    }
}