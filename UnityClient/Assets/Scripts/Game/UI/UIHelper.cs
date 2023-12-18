using Game.Framework;

namespace Game.UI
{
    public static class UIHelper
    {
        public static UIScorePanel CreateScorePanel()
        {
            return new UIScorePanel("Assets/BuildRes/UIRes/ScorePanel.prefab", UILayer.Bottom);
        }
        
        public static UIStartGamePanel CreateStartGamePanel()
        {
            return new UIStartGamePanel("Assets/BuildRes/UIRes/StartGamePanel.prefab", UILayer.Modal);
        }

        public static UIEndGamePanel CreateEndGamePanel()
        {
            return new UIEndGamePanel("Assets/BuildRes/UIRes/EndGamePanel.prefab", UILayer.Modal);
        }
    }
}