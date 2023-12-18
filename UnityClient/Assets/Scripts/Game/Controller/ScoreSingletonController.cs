using Game.Component;
using Game.UI;
using SEServer.Data;
using SEServer.GameData.Component;

namespace Game.Controller
{
    public class ScoreSingletonController : BaseSingletonController
    {
        protected override void OnUpdate()
        {
            base.OnUpdate();

            var scoreUI = UIScorePanel.Instance;
            if (scoreUI == null)
                return;

            RefreshGlobalScore(scoreUI);
            RefreshPlayerScore(scoreUI);
        }

        private void RefreshGlobalScore(UIScorePanel uiScorePanel)
        {
            var scoreGlobalView = EntityManager.GetSingletonIfExists<ScoreBoardGlobalViewComponent>();
            if (scoreGlobalView == null)
                return;
            
            uiScorePanel.SetGlobalScore(scoreGlobalView.ScoreData);
        }
        
        private void RefreshPlayerScore(UIScorePanel uiScorePanel)
        {
            var localPlayerInfo = EntityManager.GetSingleton<LocalPlayerInfoSingletonComponent>();
            if (localPlayerInfo.PlayerEntityId == EId.Invalid)
            {
                return;
            }

            var scoreView = EntityManager.GetComponent<ScoreViewComponent>(localPlayerInfo.PlayerEntityId);
            if (scoreView == null)
                return;
            
            uiScorePanel.SetScore(scoreView.Score);
        }
    }
}