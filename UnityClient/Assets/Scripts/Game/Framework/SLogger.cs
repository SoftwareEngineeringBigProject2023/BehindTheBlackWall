using System;

namespace Game.Framework
{
    public class SLogger
    {
        public SLogger(string name)
        {
            LoggerName = name;
        }

        public string LoggerName { get; private set; }
        
        public void LogInfo(object message)
        {
            UnityEngine.Debug.Log(GetLogString(message));
        }
        
        public void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning(GetLogString(message));
        }
        
        public void LogError(object message)
        {
            UnityEngine.Debug.LogError(GetLogString(message));
        }
        
        private string GetLogString(object message)
        {
            return $"[{DateTime.Now : HH:mm:ss}] [{LoggerName}] {message}";
        }
    }
}