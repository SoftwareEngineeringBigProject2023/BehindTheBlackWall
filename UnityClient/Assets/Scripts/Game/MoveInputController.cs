using System;
using SEServer.Data;
using SEServer.GameData;
using UnityEngine;

namespace Game
{
    public class MoveInputController : BaseController
    {
        public MoveInputComponent MoveInputComponent => GetEComponent<MoveInputComponent>();
        public Vector2 InputVector2 { get; set; } = Vector2.zero;
        
        private void Update()
        {
            if (MoveInputComponent.Owner != MapperManager.World.PlayerId)
            {
                return;
            }
            
            var inputX = Input.GetAxis("Horizontal");
            var inputY = Input.GetAxis("Vertical");
            var oldInput = InputVector2;
            InputVector2 = new Vector2(inputX, inputY);
            if (oldInput == InputVector2 && oldInput == Vector2.zero)
            {
                return;
            }

            MoveInputComponent.Input = InputVector2.ToSVector2();
            MoveInputComponent.IsDirty = true;
        }
    }
    
    public class MoveInputControllerBuilder : BaseControllerBuilder
    {
        public override BaseController BuildController(GameObject gameObject, IComponent component)
        {
            return gameObject.AddComponent<MoveInputController>();
        }

        public override Type BindType => typeof(MoveInputComponent);
    }
}