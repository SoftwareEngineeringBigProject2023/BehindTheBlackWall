using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Game.Framework.Editor
{
    public class ABPackerPanel : EditorWindow
    {
        [MenuItem("工具/AB打包")]
        public static void CreateWindow()
        {
            var window = GetWindow<ABPackerPanel>();
            window.titleContent = new UnityEngine.GUIContent("NextMod打包器r");
            window.Show();
        }
        
        private static bool _isDirty;

        private ABPackerCollection _collection;
        public ABPackerCollection Collection
        {
            get
            {
                if (_collection != null)
                {
                    return _collection;
                }
                
                _collection = AssetDatabase.LoadAssetAtPath("Assets/ABPackerCollection.asset", typeof(ABPackerCollection)) as ABPackerCollection;
                if (_collection == null)
                {
                    _collection = ScriptableObject.CreateInstance<ABPackerCollection>();
                    AssetDatabase.CreateAsset(_collection, "Assets/ABPackerCollection.asset");
                    AssetDatabase.SaveAssets();
                }
                
                return _collection;
            }
        }
        
        private void OnGUI()
        {
            var collection = Collection;
            // 绘制列表 Collection
            
            EditorGUILayout.LabelField("Mod打包设置");
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical(new GUIStyle("box"));

            if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus"), GUILayout.Width(80)))
            {
                var setting = new ABPackerSetting();
                collection.settings.Add(setting);
                _isDirty = true;
            }
            
            for (var i = 0; i < collection.settings.Count; i++)
            {
                var setting = collection.settings[i];
                EditorGUILayout.BeginHorizontal(new GUIStyle("box"));
                {
                    EditorGUILayout.BeginVertical();
                    {
                        setting.bundleName = InputField("AB包名：", setting.bundleName);
                        var oldAssetDir = setting.assetDir;
                        setting.assetDir = FolderInputField("文件目录：", setting.assetDir);
                        if (oldAssetDir != setting.assetDir)
                        {
                            setting.bundleName = Path.GetFileNameWithoutExtension(setting.assetDir);
                        }
                        setting.mapPath = FolderInputField("映射目录：", setting.mapPath);
                        var fullPath = string.Empty;
                        if (!string.IsNullOrEmpty(setting.assetDir))
                        {
                            fullPath = Path.GetFullPath(setting.assetDir);
                        }
                        GUILayout.Label($"完整路径：{fullPath}");
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical(GUILayout.Width(80));
                    {
                        if (GUILayout.Button("打包"))
                        {
                            DelayedCall(() => ABPackerCore.PackToAssetBundle(setting));
                        }
                        
                        if (GUILayout.Button("测试读取"))
                        {
                            DelayedCall(() => ABPackerCore.TestLoadAssetBundle(setting));
                        }
                        
                        if(GUILayout.Button("删除"))
                        {
                            collection.settings.RemoveAt(i);
                            i--;
                            _isDirty = true;
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            
            GUILayout.Space(50);
            
            if(GUILayout.Button("打包所有", GUILayout.Width(100)))
            {
                DelayedCall(() =>
                {
                    foreach (var setting in collection.settings)
                    {
                        ABPackerCore.PackToAssetBundle(setting);
                    }
                });
            }

            if (_isDirty)
            {
                _isDirty = false;
                EditorUtility.SetDirty(collection);
            }
        }
        
        public static void DelayedCall(Action action)
        {
            EditorApplication.CallbackFunction func = null;
            func = () =>
            {
                action();
                EditorApplication.delayCall -= func;
            };
            EditorApplication.delayCall += func;
        }
        
        public static string InputField(string title, string value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(title, GUILayout.Width(80));
            var result = EditorGUILayout.TextField(value);
            EditorGUILayout.EndHorizontal();
            if(result != value)
                _isDirty = true;
            
            return result;
        }
        
        public static string FolderInputField(string title, string value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(title, GUILayout.Width(80));
            var result = EditorGUILayout.TextField(value);
            if (GUILayout.Button(EditorGUIUtility.IconContent("SettingsIcon"), GUILayout.Width(20)))
            {
                // 打开文件夹选择
                var path = EditorUtility.OpenFolderPanel("选择文件夹", value, "");
                if (!string.IsNullOrEmpty(path))
                {
                    result = path;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            if(result != value)
                _isDirty = true;
            return result;
        }
    }
}