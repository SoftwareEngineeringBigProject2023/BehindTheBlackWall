using System;
using SEServer.Data;
using UnityEngine;
using ILogger = SEServer.Data.ILogger;

namespace SEServer.Client
{
    public class ClientBehaviour : MonoBehaviour
    {
        public ClientInstance ClientInstance { get; set; } = new ClientInstance();
        /// <summary>
        /// 客户端网络实例帧率
        /// </summary>
        public int MAX_FRAME_RATE = 30;
        private float _deltaTime;
        
        public void Start()
        {
            ClientInstance.ServerContainer.Add<ILogger>( new SimpleLogger());
            ClientInstance.ServerContainer.Add<IDataSerializer>( new SimpleSerializer());
            ClientInstance.ServerContainer.Add<IClientNetworkService>( new ClientNetworkService());
            ClientInstance.ServerContainer.Add<IComponentSerializer>( new ComponentSerializer());
            ClientInstance.Start();
        }
        
        private void Update()
        {
            UpdateClient();
        }

        /// <summary>
        /// 客户端帧更新
        /// </summary>
        private void UpdateClient()
        {
            _deltaTime += Time.deltaTime;
            if (_deltaTime > 1.0f / MAX_FRAME_RATE && ClientInstance.IsConnected)
            {
                ClientInstance.Update(_deltaTime);
                _deltaTime = 0;
            }
        }
    }
}