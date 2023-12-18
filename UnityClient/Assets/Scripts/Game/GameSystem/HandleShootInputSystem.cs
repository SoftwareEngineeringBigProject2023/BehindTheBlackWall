using System;
using Game.Framework;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData.Component;

namespace Game.GameSystem
{
    public class HandleShootInputSystem : ISystem
    {
        public World World { get; set; }
        public ClientWorld ClientWorld => (ClientWorld) World;
        
        public void Init()
        {
            
        }

        public void Update()
        {
            var collection = World.EntityManager.GetComponentDataCollection<ShootInputComponent>();
            var clientPlayerId = ClientWorld.PlayerId;
            foreach (var component in collection)
            {
                if (component.Owner != clientPlayerId)
                {
                    continue;
                }

                if (GameManager.Input.GetShootButtonDown())
                {
                    component.TriggerShoot = true;
                    World.MarkAsDirty(component);
                    GameManager.Input.ClearShootButtonDown();
                }
            }
            
            var interactCollection = World.EntityManager.GetComponentDataCollection<InteractInputComponent>();
            foreach (var interactInputComponent in interactCollection)
            {
                if (interactInputComponent.Owner != clientPlayerId)
                {
                    continue;
                }

                var oldIndex = interactInputComponent.SelectedWeaponIndex;
                var index = GameManager.Input.GetSelectedWeaponIndex();
                if (oldIndex != index)
                {
                    interactInputComponent.SelectedWeaponIndex = index;
                    World.MarkAsDirty(interactInputComponent);
                }
            }
        }
    }
}