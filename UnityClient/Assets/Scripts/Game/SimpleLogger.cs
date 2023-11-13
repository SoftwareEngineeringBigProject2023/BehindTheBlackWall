using Game.Framework;
using SEServer.Data;
using UnityEngine;
using ILogger = SEServer.Data.Interface.ILogger;

namespace Game
{
    public class SimpleLogger : ILogger
    {
        private SLogger _logger = new SLogger("Client");
        
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
            _logger.LogInfo(msg);
        }

        public void LogWarning(object msg)
        {
            _logger.LogWarning(msg);
        }

        public void LogError(object msg)
        {
            _logger.LogError(msg);
        }
    }
}