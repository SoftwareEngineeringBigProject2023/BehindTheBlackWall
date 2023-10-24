using System;
using System.Collections.Generic;
using UnityEngine;

namespace SEServer.Client.Editor
{
    public static class EditorGUITools
    {
        public static Stack<DisplayProgressBar> ProgressBarStack { get; set; } = new();

        public class DisplayProgressBar : IDisposable
        {
            public string Title { get; set; }
            public string Info { get; set; }
            public float Progress { get; set; }

            public void Display(float progress)
            {
                Progress = progress;
                UnityEditor.EditorUtility.DisplayProgressBar(Title, Info, progress);
            }
            
            public void Dispose()
            {
                ProgressBarStack.Pop();
                if (ProgressBarStack.Count == 0)
                {
                    UnityEditor.EditorUtility.ClearProgressBar();
                }
                else
                {
                    var top = ProgressBarStack.Peek();
                    top.Display(top.Progress);
                }
            }
        }

        public static DisplayProgressBar StartProgressBar(string title, string info, float progress)
        {
            var displayProgressBar = new DisplayProgressBar
            {
                Title = title,
                Info = info,
                Progress = progress
            };
            displayProgressBar.Display(progress);
            ProgressBarStack.Push(displayProgressBar);
            return displayProgressBar;
        }
        
        public static void SetProgress(float progress)
        {
            if (ProgressBarStack.Count == 0)
            {
                Debug.LogError("当前没有活跃的进度条。");
                return;
            }
            var top = ProgressBarStack.Peek();
            top.Display(progress);
        }
        
        public static void Set100Progress()
        {
            SetProgress(1);
        }
    }
}