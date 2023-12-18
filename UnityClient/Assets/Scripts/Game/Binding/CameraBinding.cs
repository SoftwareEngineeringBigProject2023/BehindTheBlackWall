using Cinemachine;
using UnityEngine;

namespace Game.Binding
{
    public class CameraStaticBinding
    {
        private static CinemachineVirtualCamera _virtualCamera;
        public static CinemachineVirtualCamera GetVirtualCamera()
        {
            if (_virtualCamera != null)
                return _virtualCamera;
            
            var go = GameObject.FindWithTag("VirtualCamera");
            if (go == null)
                return null;
            
            var virtualCamera = go.GetComponent<CinemachineVirtualCamera>();
            if (virtualCamera == null)
                return null;

            _virtualCamera = virtualCamera;
            return virtualCamera;
        }
    }
}