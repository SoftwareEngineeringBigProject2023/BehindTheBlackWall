using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData;

namespace Game.GameSystem
{
    public class InputSystem : ISystem
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
                
                var inputX = UnityEngine.Input.GetAxis("Horizontal");
                var inputY = UnityEngine.Input.GetAxis("Vertical");
                var oldInput = component.Input.ToUVector2();
                var input = new UnityEngine.Vector2(inputX, inputY);
                if (oldInput == input && oldInput == UnityEngine.Vector2.zero)
                {
                    continue;
                }

                component.Input = input.ToSVector2();
                World.MarkAsDirty(component);

                // 提前模拟移动
                var entity = World.EntityManager.GetEntity(component.EntityId);
                var transform = ClientWorld.EntityManager.GetComponent<TransformComponent>(entity);
                //transform.Position += input.ToSVector2() * 0.1f;
            }
        }
    }
}