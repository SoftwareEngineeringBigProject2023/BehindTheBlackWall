using System;
using Game.Framework;
using Game.Utils;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData;
using SEServer.GameData.Component;

namespace Game.GameSystem
{
    public class HandleMoveInputSystem : ISystem
    {
        public World World { get; set; }
        public ClientWorld ClientWorld => (ClientWorld) World;
        
        public void Init()
        {
            
        }

        public void Update()
        {
            var collection = World.EntityManager.GetComponentDataCollection<MoveInputComponent>();
            var clientPlayerId = ClientWorld.PlayerId;
            foreach (var component in collection)
            {
                if (component.Owner != clientPlayerId)
                {
                    continue;
                }
                
                var oldInput = component.Input.ToUVector2();
                var input = GameManager.Input.GetInputAxis();
                if (oldInput != input || oldInput != UnityEngine.Vector2.zero)
                {
                    component.Input = input.ToSVector2();
                    World.MarkAsDirty(component);

                    // 提前模拟移动，最多1帧
                    // if (ClientBehaviour.I.RecentReceiveData)
                    // {   
                    //     var entity = World.EntityManager.GetEntity(component.EntityId);
                    //     var transform = ClientWorld.EntityManager.GetComponent<TransformComponent>(entity);
                    //     transform.Position += input.ToSVector2() * 1;
                    // }
                }

                var oldAim = component.TargetRotation;
                var aim = GameManager.Input.GetAimRotation();
                if (Math.Abs(oldAim - aim) > 0.005f)
                {
                    component.TargetRotation = aim;
                    World.MarkAsDirty(component);
                }
            }
        }
    }
}