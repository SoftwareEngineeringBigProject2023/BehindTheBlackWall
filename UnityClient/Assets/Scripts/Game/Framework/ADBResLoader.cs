#if UNITY_EDITOR
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Game.Framework
{
    public class ADBResLoader : IResLoader
    {
        public void Reset()
        {
            
        }

        public Object Load(string resPath)
        {
            return AssetDatabase.LoadAssetAtPath(resPath, typeof(Object));
        }

        public UniTask<Object> LoadAsync(string resPath)
        {
            return UniTask.FromResult(Load(resPath));
        }

        public byte[] LoadBytes(string resPath)
        {
            return AssetDatabase.LoadAssetAtPath<TextAsset>(resPath).bytes;
        }

        public UniTask<byte[]> LoadBytesAsync(string resPath)
        {
            return UniTask.FromResult(LoadBytes(resPath));
        }

        public string LoaderCode => "AssetDatabase";
    }
}
#endif