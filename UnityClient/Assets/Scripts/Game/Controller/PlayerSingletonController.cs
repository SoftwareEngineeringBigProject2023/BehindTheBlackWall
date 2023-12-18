using System.Linq;
using Cinemachine;
using Game.Binding;
using Game.Component;
using Game.Framework;
using Game.Utils;
using SEServer.GameData.Component;
using UnityEngine;

namespace Game.Controller
{
    public class PlayerSingletonController : BaseSingletonController
    {
        
        private LocalPlayerInfoSingletonComponent _localPlayerInfo;
        public LocalPlayerInfoSingletonComponent LocalPlayerInfo
        {
            get
            {
                if(_localPlayerInfo == null)
                    _localPlayerInfo = EntityManager.GetSingleton<LocalPlayerInfoSingletonComponent>();
                
                return _localPlayerInfo;
            }
        }

        protected override void OnUpdate()
        {
            // 移动输入
            MoveInput();
            
            // 瞄准输入
            AimInput();
            
            // 射击输入
            ShootInput();
            
            // 交互输入
            InteractInput();
            
            // 摄像头跟随
            CameraFollow();
        }

        private void InteractInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GameManager.Input.SetSelectedWeaponIndex(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                GameManager.Input.SetSelectedWeaponIndex(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                GameManager.Input.SetSelectedWeaponIndex(2);
            }
        }

        private void MoveInput()
        {
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");
            GameManager.Input.SetInputAxis(new Vector2(x, y));
        }
        
        private void AimInput()
        {
            var mousePosition = Input.mousePosition;
            var entityManager = EntityManager;

            var playerComponent = entityManager.GetComponent<PlayerComponent>(LocalPlayerInfo.PlayerEntityId);
            if(playerComponent == null)
                return;
            
            var transformComponent = entityManager.Components.GetComponent<TransformComponent>(playerComponent.EntityId);
            if(transformComponent == null)
                return;
            
            var camera = Camera.main;
            if(camera == null)
                return;
            
            var screenPoint = camera.WorldToScreenPoint(transformComponent.Position.ToUVector2());
            var offset = (Vector2)(mousePosition - screenPoint);
            GameManager.Input.SetAimAxis(offset.normalized);
        }
        
        private void ShootInput()
        {
            if (Input.GetMouseButton(0))
            {
                GameManager.Input.TriggerShootButtonDown();
            }
        }
        
        private void CameraFollow()
        {
            var virtualCamera = CameraStaticBinding.GetVirtualCamera();
            if(virtualCamera == null)
                return;
            
            var entityManager = EntityManager;
            
            var playerComponent = entityManager.GetComponent<PlayerComponent>(LocalPlayerInfo.PlayerEntityId);
            if (playerComponent == null)
                return;

            if (virtualCamera.Follow != null)
                return;
            
            virtualCamera.Follow = ClientBehaviour.I.EntityMapper
                .GetEController<GraphController>(playerComponent.EntityId).GraphObject.transform;
        }
    }
}