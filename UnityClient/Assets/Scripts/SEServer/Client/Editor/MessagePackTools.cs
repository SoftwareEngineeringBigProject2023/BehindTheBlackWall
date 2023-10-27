using System.Collections;
using System.Collections.Generic;
using System.IO;
using SEServer.Client.Editor;
using UnityEditor;
using UnityEngine;

public static class MessagePackTools
{
    [MenuItem("工具/复制数据DLL")]
    public static void CopyDllFile()
    {
        var dllDirPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../../SEServer/SEServer.GameData/bin/Debug/net48"));
        var copyFiles = new List<string>()
        {
            Path.Combine(dllDirPath, "SEServer.GameData.dll"),
            Path.Combine(dllDirPath, "SEServer.GameData.pdb"),
            Path.Combine(dllDirPath, "SEServer.Data.dll"),
            Path.Combine(dllDirPath, "SEServer.Data.pdb"),
        };
        
        var destDirPath = Path.GetFullPath(Path.Combine(Application.dataPath, "SEServerDll/"));
        if (!Directory.Exists(destDirPath))
            Directory.CreateDirectory(destDirPath);

        foreach (var copyFile in copyFiles)
        {
            var destFilePath = Path.Combine(destDirPath, Path.GetFileName(copyFile));
            File.Copy(copyFile, destFilePath, true);
            Debug.Log($"复制文件：{copyFile} -> {destFilePath}");
        }
        
        AssetDatabase.Refresh();
    }
    
    [MenuItem("工具/生成MessagePack AOT代码")]
    public static void GenerateAOTCode()
    {
        using (EditorGUITools.StartProgressBar("生成MessagePack AOT代码", "正在生成MessagePack AOT代码", 0))
        {
            var sb = new System.Text.StringBuilder();
            var rootPath = Application.dataPath;
            var repoPath = Path.GetFullPath(Path.Combine(rootPath, "../../"));
            sb.AppendLine($"cd {repoPath}");
            
            var projDataPath = Path.GetFullPath(Path.Combine(repoPath, "SEServer/SEServer.Data/SEServer.Data.csproj"));
            var projGameDataPath = Path.GetFullPath(Path.Combine(repoPath, "SEServer/SEServer.GameData/SEServer.GameData.csproj"));
            var outputDir = Path.GetFullPath(Path.Combine(rootPath, "MessagePackGen"));
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);
            var outputDataPath = Path.GetFullPath(Path.Combine(outputDir, "MessagePack.SEServer.Data.Generated.cs"));
            var outputGameDataPath = Path.GetFullPath(Path.Combine(outputDir, "MessagePack.SEServer.GameData.Generated.cs"));
            
            sb.AppendLine($"dotnet mpc -i \"{projDataPath}\" -o \"{outputDataPath}\" -r \"GeneratedDataResolver\" -n \"MessagePack.SEServer.Data\"");
            sb.AppendLine($"dotnet mpc -i \"{projGameDataPath}\" -o \"{outputGameDataPath}\" -r \"GeneratedGameDataResolver\"  -n \"MessagePack.SEServer.GameData\"");
            
            EditorGUITools.SetProgress(0.5f);
            ShellTools.RunShellOnDisk(sb.ToString());
            EditorGUITools.Set100Progress();
            
            AssetDatabase.Refresh();
        }
    }
}
