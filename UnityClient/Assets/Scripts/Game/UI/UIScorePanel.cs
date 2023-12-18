using System.Collections.Generic;
using System.Text;
using Game.Framework;
using SEServer.GameData.Component;
using TMPro;

namespace Game.UI
{
    public class UIScorePanel : UIBase
    {
        public UIScorePanel(string prefabPath, UILayer uiLayer) : base(prefabPath, uiLayer)
        {
        }
        
        public static UIScorePanel Instance { get; private set; }
        public static int HighScore { get; set; }
        public static int CurrentScore { get; set; }

        public TMP_Text txtScore;
        public TMP_Text txtGlobalScore;
        
        protected override void OnInit()
        {
            base.OnInit();
            
            txtScore = Get<TMP_Text>("txtScore");
            txtGlobalScore = Get<TMP_Text>("txtGlobalScore");
            
            txtScore.text = "";
            txtGlobalScore.text = "";
        }

        protected override void OnShown()
        {
            base.OnShown();
            Instance = this;
        }
        
        public void SetScore(int score)
        {
            if (score > HighScore)
            {
                HighScore = score;
            }

            CurrentScore = score;
            txtScore.text = $"分数：{score}";
        }
        
        public void SetGlobalScore(List<ScoreData> scores)
        {
            var sb = new StringBuilder();
            sb.AppendLine("排行榜");
            int rank = 1;
            foreach (var score in scores)
            {
                sb.AppendLine($"{rank++}. <indent=15%>{score.Score}</indent> <indent=50%>{score.Name}</indent>");
            }
            txtGlobalScore.text = sb.ToString();
        }
    }
}