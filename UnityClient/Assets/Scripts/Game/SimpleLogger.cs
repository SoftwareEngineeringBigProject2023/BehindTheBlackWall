using SEServer.Data;
using UnityEngine;
using ILogger = SEServer.Data.Interface.ILogger;

namespace Game
{
    public class SimpleLogger : ILogger
    {
        public void Init()
        {
            
        }

        public void Start()
        {
            
        }

        public void Stop()
        {
            
        }

        public ServerContainer ServerContainer { get; set; }
        public void LogInfo(object msg)
        {
            Debug.Log(msg);
        }

        public void LogWarning(object msg)
        {
            Debug.LogWarning(msg);
        }

        public void LogError(object msg)
        {
            Debug.LogError(msg);
        }
    }
}