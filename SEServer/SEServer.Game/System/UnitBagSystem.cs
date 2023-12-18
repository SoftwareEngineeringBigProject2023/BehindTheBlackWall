using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData.Component;
using SEServer.GameData.CTData;

namespace SEServer.Game.System;

public class UnitBagSystem : ISystem
{
    public World World { get; set; }
    public void Init()
    {
        
    }

    public void Update()
    {
        var collection = World.EntityManager.GetComponentDataCollection<InteractInputComponent, BagComponent>();
        foreach (var valueTuple in collection)
        {
            var (interactInput, bag) = valueTuple;
            if(interactInput.SelectedWeaponIndex != bag.SelectedWeaponIndex || bag.NeedRefresh)
            {
                if (interactInput.SelectedWeaponIndex < 0 || interactInput.SelectedWeaponIndex >= bag.Weapons.Count)
                {
                    interactInput.SelectedWeaponIndex = 0;
                }
                
                bag.SelectedWeaponIndex = interactInput.SelectedWeaponIndex;
                World.EntityManager.MarkAsChanged(bag);
                
                // 选择武器
                var weapon = World.EntityManager.GetComponent<WeaponComponent>(bag.EntityId);
                if (weapon != null)
                {
                    var item = bag.Weapons.TryGet(bag.SelectedWeaponIndex);
                    if(item != null && item.BindType == BagItemType.Weapon)
                    {
                        weapon.WeaponId = item.BindId;
                        weapon.WeaponCTData = World.ServerContainer.Get<IConfigTable>().Get<WeaponCTData>(item.BindId);
                    }
                    else
                    {
                        weapon.WeaponId = "";
                        weapon.WeaponCTData = null;
                    }
                    
                    World.EntityManager.MarkAsChanged(weapon);
                }
                
                bag.NeedRefresh = false;
            }
        }
    }
}