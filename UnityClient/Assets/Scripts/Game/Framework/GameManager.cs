using System;
using Cysharp.Threading.Tasks;

namespace Game.Framework
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public static ResManager Res => I.ResManager;
        public static UIManager UI => I.UIManager;
        public static AudioManager Audio => I.AudioManager;
        public static InputManager Input => I.InputManager;
        
        private bool _isInit;
        public ResManager ResManager { get; set; }
        public UIManager UIManager { get; set; }
        public AudioManager AudioManager { get; set; }
        public InputManager InputManager { get; set; }
        public bool IsInit => _isInit;
        
        public async UniTask Init()
        {
            if(_isInit)
                return;
            
            _isInit = true;
            
            ResManager = ResManager.Instance;
            UIManager = UIManager.Instance;
            AudioManager = AudioManager.Instance;
            InputManager = InputManager.Instance;
            
            await ResManager.Init();
            UIManager.Init();
        }

        public void SwitchToScene(string scene)
        {
            ResManager.LoadScene(scene);
        }
    }
}