using System;
using Game.Framework;
using UnityEngine;

namespace Game.SceneScript
{
    public class InitScene : MonoBehaviour
    {
        private void Start()
        {
            var gameManager = GameManager.Instance;
            gameManager.Init();

            gameManager.SwitchToScene("GameTest");
        }
    }
}