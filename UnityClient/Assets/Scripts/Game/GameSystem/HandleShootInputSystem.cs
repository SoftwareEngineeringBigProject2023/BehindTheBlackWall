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
        }
    }
}