using System;
using UnityEngine;

namespace Game.Binding
{
    public class GameEffectBinding : MonoBehaviour
    {
        public float LifeTime { get; set; } = 2f;
        public bool IsPlayer { get; set; }

        private void Update()
        {
            LifeTime -= Time.deltaTime;

            if (LifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}