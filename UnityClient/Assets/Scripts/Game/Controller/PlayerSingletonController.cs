using System.Linq;
using Game.Framework;
using SEServer.GameData.Component;
using UnityEngine;

namespace Game.Controller
{
    public class PlayerSingletonController : BaseSingletonController
    {
        protected override void OnUpdate()
        {
            // 移动输入
            MoveInput();
            
            // 瞄准输入
            AimInput();
            
            // 射击输入
            ShootInput();
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
            var entityManager = ClientBehaviour.I.ClientInstance.World.EntityManager;
            
            var playerComponent = entityManager.GetComponentDataCollection<PlayerComponent>()
                .FirstOrDefault(player => player.PlayerId == ClientBehaviour.I.ClientInstance.World.PlayerId.Id);
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
    }
}