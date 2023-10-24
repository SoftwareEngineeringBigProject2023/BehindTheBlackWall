using System;
using System.IO;
using UnityEngine;

namespace SEServer.Client.Editor
{
    public static class ShellTools
    {
        public static void RunShellOnDisk(string shellContent)
        {
            // 随机生成一个临时文件名
            var tempShellPath = Path.Combine(Application.temporaryCachePath, $"{Guid.NewGuid()}.bat");
            Debug.Log($"生成临时Shell文件：{tempShellPath}");
            File.WriteAllText(tempShellPath, shellContent);
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/c {tempShellPath}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            Debug.Log($"Shell执行结果：{output}");
            
            File.Delete(tempShellPath);
        }
    }
}