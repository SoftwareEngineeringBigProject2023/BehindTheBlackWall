using System.Text.RegularExpressions;
using Game.Framework;
using Game.SceneScript;
using Game.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIStartGamePanel : UIBase
    {
        public UIStartGamePanel(string prefabPath, UILayer uiLayer) : base(prefabPath, uiLayer)
        {
        }

        public TMP_Text txtTips;
        public TMP_InputField inName;
        public Button btnStart;
        public Button btnPrev;
        public Button btnNext;
        public Image imgIcon;

        public int iconIndex = 1;
        
        public const int ICON_COUNT = 12;
        
        protected override void OnInit()
        {
            base.OnInit();
            
            txtTips = Get<TMP_Text>("txtTips");
            inName = Get<TMP_InputField>("inName");
            btnStart = Get<Button>("btnStart");

            btnPrev = Get<Button>("btnPrev");
            btnNext = Get<Button>("btnNext");
            imgIcon = Get<Image>("imgIcon");
            
            txtTips.text = "";
            inName.text = RandomName();
            btnStart.onClick.AddListener(OnClickStart);

            btnPrev.onClick.AddListener(OnClickPrev);
            btnNext.onClick.AddListener(OnClickNext);
            
            iconIndex = RandomHeadIcon();
            RefreshIcon();
        }

        private void OnClickNext()
        {
            iconIndex += 1;
            if (iconIndex > ICON_COUNT)
                iconIndex = 1;
            RefreshIcon();
        }

        private void OnClickPrev()
        {
            iconIndex -= 1;
            if (iconIndex < 1)
                iconIndex = ICON_COUNT;
            RefreshIcon();
        }

        private void RefreshIcon()
        {
            var icon = GameManager.Res.LoadAsset<Sprite>($"Assets/BuildRes/HeadIcon/{iconIndex}.png");
            imgIcon.sprite = icon;
        }

        private void OnClickStart()
        {
            var name = inName.text;
            if (string.IsNullOrEmpty(name))
            {
                txtTips.text = "请输入昵称";
                return;
            }
            
            if(name.Length > 8)
            {
                txtTips.text = "昵称不能超过8个字符";
                return;
            }

            GameTestScene.Instance.StartGame(name, iconIndex);
            Hide();
        }

        private string[] prefix = new string[]
        {
            "大名鼎鼎的",
            "无敌的",
            "厉害的",
            "牛逼的",
            "狂拽酷炫的",
            "帅气的",
            "漂亮的",
            "熟练的",
        };
        
        private string[] postfix = new string[]
        {
            "V",
            "杰克",
            "强尼",
            "露丝",
            "大卫",
            "瑞贝卡",
            "竹村",
        };
        
        private string RandomName()
        {
            var pre = prefix[RandomHelper.RandomInt(0, prefix.Length)];
            var post = postfix[RandomHelper.RandomInt(0, postfix.Length)];
            return $"{pre}{post}";
        }

        private int RandomHeadIcon()
        {
            return RandomHelper.RandomInt(1, ICON_COUNT);
        }
    }
}