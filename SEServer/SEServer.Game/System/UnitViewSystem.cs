using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData.Component;

namespace SEServer.Game.System;

/// <summary>
/// 该System用于将玩家的数据组件同步到显示组件上
/// </summary>
public class UnitViewSystem : ISystem
{
    public World World { get; set; }
    public void Init()
    {
        
    }

    public void Update()
    {
        var collection = World.EntityManager.GetComponentDataCollection<PropertyComponent>();
        foreach (var propertyComponent in collection)
        {
            var eId = propertyComponent.EntityId;
            var healthComponent = World.EntityManager.GetComponent<UnitHealthViewComponent>(eId);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (healthComponent != null && healthComponent.Hp != propertyComponent.Hp)
            {
                healthComponent.HpMax = propertyComponent.HpMax;
                healthComponent.Hp = propertyComponent.Hp;
                World.EntityManager.MarkAsDirty(healthComponent);
            }
            
            var aimComponent = World.EntityManager.GetComponent<UnitAimViewComponent>(eId);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (aimComponent != null && aimComponent.TargetAimRotation != propertyComponent.TargetAimRotation)
            {
                aimComponent.TargetAimRotation = propertyComponent.TargetAimRotation;
                World.EntityManager.MarkAsDirty(aimComponent);
            }
        }
    }
}