using System;
using Game.Binding;
using SEServer.GameData.Component;
using UnityEngine;

namespace Game.Controller
{
    public class UnitGraphController : BaseComponentController
    {
        public UnitGraphController()
        {
            GraphController = new LazyControllerGetter<GraphController>(this);
        }

        public LazyControllerGetter<GraphController> GraphController { get; set; }
        public PropertyComponent PropertyComponent => GetEComponent<PropertyComponent>();
        public UnitBinding UnitBinding { get; set; }

        protected override void OnUpdate()
        {
            var graphController = GraphController.Value;
            if (graphController == null)
                return;

            if (UnitBinding == null)
                UnitBinding = graphController.GraphObject.GetComponent<UnitBinding>();

            var propertyComponent = PropertyComponent;
            if (propertyComponent == null)
                return;

            var rotateAngle = propertyComponent.TargetAimRotation % 360f;
            if (rotateAngle < 0)
            {
                rotateAngle += 360f;
            }


            var rotateRoot = UnitBinding.weaponRotateRoot;
            if (rotateRoot != null)
            {
                var unitWeaponTransform = UnitBinding.weaponRotateRoot.transform;
                if (rotateAngle < 90f || rotateAngle > 270f)
                {
                    unitWeaponTransform.localScale = new Vector3(1, 1, 1);
                    if(rotateAngle > 270f)
                        rotateAngle -= 360f;
                    TryLerpToTargetRotation(unitWeaponTransform, rotateAngle);
                }
                else
                {
                    unitWeaponTransform.localScale = new Vector3(-1, 1, 1);
                    TryLerpToTargetRotation(unitWeaponTransform, rotateAngle - 180, -1);
                }
            }

        }

        private int lastScale = 1;

        /// <summary>
        /// 平滑旋转到目标角度
        /// </summary>
        /// <param name="unitWeaponTransform"></param>
        /// <param name="rotateAngle"></param>
        /// <param name="scale"></param>
        private void TryLerpToTargetRotation(Transform unitWeaponTransform, float rotateAngle, int scale = 1)
        {
            //Debug.Log(rotateAngle);
            if (lastScale != scale)
            {
                var curRotateAngle = unitWeaponTransform.rotation.eulerAngles.z;
                unitWeaponTransform.rotation = Quaternion.Euler(0, 0, -curRotateAngle);
                lastScale = scale;
            }
            
            // 使用线性插值
            var targetRotation = Quaternion.Euler(0, 0, rotateAngle);
            unitWeaponTransform.rotation = Quaternion.Lerp(unitWeaponTransform.rotation, targetRotation, Time.deltaTime * 10);
        }
    }

    public class UnitGraphControllerBuilder : BaseControllerBuilder
    {
        public override Type BindType { get; } = typeof(PropertyComponent);

        public override BaseComponentController BuildController()
        {
            return new UnitGraphController();
        }
    }
}