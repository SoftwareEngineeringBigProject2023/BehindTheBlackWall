using System;

namespace Game.Framework
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public ResManager ResManager { get; set; }
        public UIManager UIManager { get; set; }
        public AudioManager AudioManager { get; set; }
        
        public static ResManager Res => I.ResManager;
        public static UIManager UI => I.UIManager;
        public static AudioManager Audio => I.AudioManager;

        public void Init()
        {
            ResManager = ResManager.Instance;
            UIManager = UIManager.Instance;
            AudioManager = AudioManager.Instance;
            
            ResManager.Init();
            UIManager.Init();
        }

        public void SwitchToScene(string scene)
        {
            ResManager.LoadScene(scene);
        }
    }
}