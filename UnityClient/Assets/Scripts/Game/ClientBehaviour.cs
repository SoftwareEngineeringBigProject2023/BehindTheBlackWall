using System.Collections.Generic;
using SEServer.Client;
using SEServer.Data;
using UnityEngine;
using ILogger = SEServer.Data.ILogger;

namespace Game
{
    public class ClientBehaviour : MonoBehaviour
    {
        public static ClientBehaviour Instance { get; private set; }
        public ClientInstance ClientInstance { get; set; }
        /// <summary>
        /// 客户端网络实例帧率
        /// </summary>
        public static int MAX_FRAME_RATE = 15;
        public List<IClientAttachBehaviour> attachBehaviours = new();
        private float _deltaTime;
        
        public void Start()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
            
            CheckBehaviours();
            
            ClientInstance = new ClientInstance();  
            ClientInstance.ServerContainer.Add<ILogger>( new SimpleLogger());
            ClientInstance.ServerContainer.Add<IDataSerializer>( new SimpleSerializer());
            ClientInstance.ServerContainer.Add<IClientNetworkService>( new ClientNetworkService());
            ClientInstance.ServerContainer.Add<IComponentSerializer>( new ComponentSerializer());
            ClientInstance.Start();
        }

        private void CheckBehaviours()
        {
            var allBehaviours = GetComponentsInChildren<IClientAttachBehaviour>();
            foreach (var attachBehaviour in allBehaviours)
            {
                attachBehaviour.ClientBehaviour = this;
                attachBehaviours.Add(attachBehaviour);
            }
        }

        private void Update()
        {
            UpdateClient();
            UpdateBehaviours();
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
        
        private void UpdateBehaviours()
        {
            foreach (var behaviour in attachBehaviours)
            {
                behaviour.UpdateBehaviour();
            }
        }
    }
}