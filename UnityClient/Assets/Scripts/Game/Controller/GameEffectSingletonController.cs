using DamageNumbersPro;
using Game.Binding;
using Game.Component;
using Game.Framework;
using Game.GameComponent;
using UnityEngine;

namespace Game.Controller
{
    public class GameEffectSingletonController : BaseSingletonController
    {
        protected override void OnUpdate()
        {
            base.OnUpdate();

            var graphRequestCom = EntityManager.GetSingleton<GraphRequestGlobalComponent>();

            while (graphRequestCom.Requests.TryDequeue(out var request))
            {
                var unitGraphController = MapperManager.GetEController<UnitGraphController>(request.Transform.EntityId);
                if (unitGraphController == null)
                    break;
                
                switch (request)
                {
                    case ShootRequest shootRequest:
                    {
                        HandleShootRequest(shootRequest, unitGraphController);
                        break;
                    }
                    case ScoreChangeRequest scoreChangeRequest:
                    {
                        HandleScoreChangedRequest(scoreChangeRequest, unitGraphController);
                        break;
                    }
                    case InjuryRequest injuryRequest:
                    {
                        HandleInjuryRequest(injuryRequest, unitGraphController);
                        break;
                    }
                }
            }
        }

        private static void HandleInjuryRequest(InjuryRequest injuryRequest, UnitGraphController unitGraphController)
        {
            if (injuryRequest.Damage <= 0)
                return;

            unitGraphController.UnitBinding.unitInjuryFeedback.PlayFeedbacks();

            var position = unitGraphController.GetUnitHeadPos();

            var damagePrefab =
                GameManager.Res.LoadAsset<GameObject>("Assets/BuildRes/Prefab/PopDamage.prefab").GetComponent<DamageNumber>();
            damagePrefab.Spawn(position,
                injuryRequest.Damage,
                unitGraphController.UnitBinding.transform);
        }

        private static void HandleScoreChangedRequest(ScoreChangeRequest scoreChangeRequest,
            UnitGraphController unitGraphController)
        {
            if (scoreChangeRequest.Score <= 0)
                return;

            unitGraphController.UnitBinding.unitPickupFeedback.PlayFeedbacks();

            var position = unitGraphController.GetUnitHeadPos();

            var scorePrefab =
                GameManager.Res.LoadAsset<GameObject>("Assets/BuildRes/Prefab/PopScore.prefab").GetComponent<DamageNumber>();
            scorePrefab.Spawn(position,
                scoreChangeRequest.Score.ToString());
        }

        private void HandleShootRequest(ShootRequest shootRequest, UnitGraphController unitGraphController)
        {
            if (shootRequest.WeaponCtData.StartFlame <= 0)
                return;
            
            if (unitGraphController.UnitBinding == null)
                return;

            var position = unitGraphController.GetMuzzlePos();
            var rotate = new Vector3(0, 0, unitGraphController.GetGunRotate());

            var effectFlame = GetFlame(shootRequest.WeaponCtData.StartFlame);
            effectFlame.LifeTime = 1f;

            var localPlayerInfo = EntityManager.GetSingleton<LocalPlayerInfoSingletonComponent>();

            effectFlame.IsPlayer = shootRequest.Transform.EntityId == localPlayerInfo.PlayerEntityId;

            var transform = effectFlame.transform;
            transform.position = position;
            transform.eulerAngles = rotate;

            unitGraphController.UnitBinding.weaponFileFeedback.PlayFeedbacks();

            return;
        }

        private GameEffectBinding GetFlame(int startFlame)
        {
            var prefabPath = "Assets/BuildRes/Prefab/Flame_1.prefab";
            switch (startFlame)
            {
                case 1:
                case 2:
                case 3:
                    prefabPath = $"Assets/BuildRes/Prefab/Flame_{startFlame}.prefab";
                    break;
            }

            var go = GameManager.Res.Spawn(prefabPath);
            var effect = go.AddComponent<GameEffectBinding>();

            return effect;
        }
    }
}