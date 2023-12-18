using System.IO;
using UnityEngine;

namespace Game.Utils
{
    public static class FileUtils
    {
        public static void CopyDirectory(string sourcePath, string destinationPath, string matchPattern = "*.*")
        {
            var sourceDirectoryInfo = new DirectoryInfo(sourcePath);
            var destinationDirectoryInfo = new DirectoryInfo(destinationPath);

            if (!sourceDirectoryInfo.Exists)
            {
                Debug.LogError($"Source directory does not exist or could not be found: {sourcePath}");
                return;
            }

            if (!destinationDirectoryInfo.Exists)
            {
                destinationDirectoryInfo.Create();
            }

            foreach (var fileInfo in sourceDirectoryInfo.GetFiles(matchPattern))
            {
                Debug.Log($"Copying {fileInfo.Name} to {destinationDirectoryInfo.FullName}");
                fileInfo.CopyTo(Path.Combine(destinationDirectoryInfo.FullName, fileInfo.Name), true);
            }

            foreach (var directoryInfo in sourceDirectoryInfo.GetDirectories())
            {
                CopyDirectory(directoryInfo.FullName, Path.Combine(destinationDirectoryInfo.FullName, directoryInfo.Name), matchPattern);
            }
        }
    }
}