using System;
using Cysharp.Threading.Tasks;
using Game.Framework;
using UnityEngine;

namespace Game.SceneScript
{
    public class GameTestScene : MonoBehaviour
    {
        private void Start()
        {
            StartGame();
        }

        private async void StartGame()
        {
            var gameManager = GameManager.Instance;
            if (!gameManager.IsInit)
            {
                gameManager.Init();
            }
            
            ClientBehaviour.I.Init();
            await ClientBehaviour.I.Connect();
            await UniTask.WaitUntil(() => ClientBehaviour.I.ClientInstance.UserId != -1);
            ClientBehaviour.I.TestAddPlayer();
        }
    }
}