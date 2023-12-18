using System.Collections.Generic;
using Game.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Framework
{
    public enum UILayer
    {
        Bottom,
        Window,
        Modal
    }
    
    public class UIManager : MonoSingleton<UIManager>
    {
        public Transform UIRoot { get; private set; }
        
        public Transform UILayerBottom { get; private set; }
        
        public Transform UILayerWindow { get; private set; }
        
        public Transform UILayerModal { get; private set; }
        
        private List<UIBase> _bottomUIs = new List<UIBase>();
        private List<UIBase> _windowUIs = new List<UIBase>();
        private List<UIBase> _modalUIs = new List<UIBase>();
        
        public void Init()
        {
            BuildUIRoot();
        }

        private void BuildUIRoot()
        {
            var goRoot = new GameObject("UIRoot").transform;
            UIRoot = goRoot.gameObject.AddComponent<RectTransform>();
            UIRoot.SetParent(transform);
            var canvas = UIRoot.gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = UIRoot.gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            var raycaster = UIRoot.gameObject.AddComponent<GraphicRaycaster>();
            raycaster.ignoreReversedGraphics = true;
            raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

            UILayerBottom = CreateLayer(UILayer.Bottom);
            UILayerWindow = CreateLayer(UILayer.Window);
            UILayerModal = CreateLayer(UILayer.Modal);
            
            var eventSystem = new GameObject("EventSystem");
            eventSystem.transform.SetParent(UIRoot);
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
        
        private Transform CreateLayer(UILayer layer)
        {
            var go = new GameObject($"UILayer{layer}");
            var rectTransform = go.AddComponent<RectTransform>();
            rectTransform.SetParent(UIRoot);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.localPosition = Vector3.zero;
            return rectTransform;
        }
        
        public void RegisterUI<T>(T ui) where T : UIBase
        {
            var list = GetListByLayer(ui.Layer);
            list.Add(ui);
            
            ui.BindGameObject.transform.SetParent(GetLayerTransform(ui.Layer), false);
        }
        
        public void UnregisterUI<T>(T ui) where T : UIBase
        {
            var list = GetListByLayer(ui.Layer);
            list.Remove(ui);
            
            ui.BindGameObject.transform.SetParent(null);
        }
        
        public void RemoveAllUI()
        {
            foreach (var ui in _bottomUIs.ToArray())
            {
                ui?.Hide();
            }
            _bottomUIs.Clear();
            
            foreach (var ui in _windowUIs.ToArray())
            {
                ui?.Hide();
            }
            _windowUIs.Clear();
            
            foreach (var ui in _modalUIs.ToArray())
            {
                ui?.Hide();
            }
            _modalUIs.Clear();
        }
        
        public void RemoveUIByLayer(UILayer layer)
        {
            var list = GetListByLayer(layer);
            foreach (var ui in list.ToArray())
            {
                ui.Hide();
            }
            list.Clear();
        }
        
        public List<UIBase> GetListByLayer(UILayer layer)
        {
            switch (layer)
            {
                case UILayer.Bottom:
                    return _bottomUIs;
                case UILayer.Window:
                    return _windowUIs;
                case UILayer.Modal:
                    return _modalUIs;
                default:
                    throw new System.Exception($"不存在的UI层级：{layer}");
            }
        }

        public Transform GetLayerTransform(UILayer layer)
        {
            switch (layer)
            {
                case UILayer.Bottom:
                    return UILayerBottom;
                case UILayer.Window:
                    return UILayerWindow;
                case UILayer.Modal:
                    return UILayerModal;
                default:
                    throw new System.Exception($"不存在的UI层级：{layer}");
            }
        }
    }
}