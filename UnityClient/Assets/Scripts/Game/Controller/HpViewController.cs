using System;
using DG.Tweening;
using Game.Binding;
using Game.Framework;
using SEServer.GameData.Component;
using UnityEngine;

namespace Game.Controller
{
    public class HpViewController : BaseComponentController
    {
        public UnitHealthViewComponent HealthViewComponent => GetEComponent<UnitHealthViewComponent>();
        public GraphController GraphController => GetEController<GraphController>();
        public HpBarBinding HpBar { get; set; }
        private float RecordHp { get; set; }

        protected override void OnStart()
        {
            base.OnStart();
            
            var graphRoot = GraphController.GraphObject.transform;
            HpBar = GameManager.Res.Spawn("Assets/BuildRes/Prefab/HpBar.prefab", graphRoot).GetComponent<HpBarBinding>();
            HpBar.GetComponent<Canvas>().sortingOrder = 100;
            RefreshHpBar(false);
        }
    
        private void RefreshHpBar(bool useAnim = true)
        {
            var health = HealthViewComponent;
            if (health == null)
                return;
            
            var slider = HpBar.slider;
            slider.maxValue = health.HpMax;
            slider.minValue = 0;
            if (useAnim)
            {
                HpBar.slider.DOKill();
                HpBar.slider.DOValue(health.Hp, 0.5f);
            }
            else
            {
                HpBar.slider.value = health.Hp;
            }
            RecordHp = health.Hp;
        }

        
        protected override void OnUpdate()
        {
            base.OnUpdate();
            
            if (Math.Abs(HealthViewComponent.Hp - RecordHp) > 0.05f)
            {
                RecordHp = HealthViewComponent.Hp;
                RefreshHpBar();
            }
        }
    }
    
    public class HpViewControllerBuilder : BaseControllerBuilder
    {
        public override Type BindType { get; } = typeof(UnitHealthViewComponent);
        
        public override BaseComponentController BuildController()
        {
            return new HpViewController();
        }
    }
}