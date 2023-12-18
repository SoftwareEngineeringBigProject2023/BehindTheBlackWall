using System;
using Game.Framework;
using UnityEngine;

namespace Game.SceneScript
{
    public class InitScene : MonoBehaviour
    {
        private async void Start()
        {
            var gameManager = GameManager.Instance;
            await gameManager.Init();

            gameManager.SwitchToScene("GameTest");
        }
    }
}