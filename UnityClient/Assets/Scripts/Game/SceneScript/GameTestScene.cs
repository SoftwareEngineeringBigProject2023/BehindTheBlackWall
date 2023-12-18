using System;
using Cysharp.Threading.Tasks;
using Game.Framework;
using Game.UI;
using UnityEngine;

namespace Game.SceneScript
{
    public class GameTestScene : MonoBehaviour
    {
        public static GameTestScene Instance { get; private set; }
        
        private UIScorePanel score;
        
        private void Start()
        {
            Instance = this;
            InitGame();
        }

        private async void InitGame()
        {
            var gameManager = GameManager.Instance;
            if (!gameManager.IsInit)
            {
                await gameManager.Init();
            }
            
            ClientBehaviour.I.Init();
            await ClientBehaviour.I.Connect();
            await UniTask.WaitUntil(() => ClientBehaviour.I.ClientInstance.UserId != -1);
            
            var startGamePanel = UIHelper.CreateStartGamePanel();
            startGamePanel.Show();
        }

        public void StartGame(string playerName, int iconIndex)
        {
            ClientBehaviour.I.TestAddPlayer(playerName, iconIndex);
            
            score = UIHelper.CreateScorePanel();
            score.Show();
        }

        public void EndGame()
        {
            score.Hide();

            var endGamePanel = UIHelper.CreateEndGamePanel();
            endGamePanel.Show();
        }

        public void RestartGame()
        {
            var startGamePanel = UIHelper.CreateStartGamePanel();
            startGamePanel.Show();
        }
    }
}