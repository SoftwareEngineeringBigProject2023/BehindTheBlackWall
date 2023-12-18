using Game.Framework;
using Game.SceneScript;
using TMPro;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIEndGamePanel : UIBase
    {
        public UIEndGamePanel(string prefabPath, UILayer uiLayer) : base(prefabPath, uiLayer)
        {
        }

        protected override void OnInit()
        {
            base.OnInit();

            var btnRestart = Get<Button>("btnRestart");
            btnRestart.onClick.AddListener(OnClickRestart);
            var txtScore = Get<TMP_Text>("txtScore");
            txtScore.text = $"你当前的分数为：{UIScorePanel.CurrentScore}";
            txtScore.text = $"你最高的分数为：{UIScorePanel.HighScore}";
        }

        private void OnClickRestart()
        {
            GameTestScene.Instance.RestartGame();
            Hide();
        }
    }
}