using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Binding
{
    public class UnitBinding : MonoBehaviour
    {
        public SpriteRenderer headIcon;
        public Transform graphRoot;
        public Transform weaponRotateRoot;
        public Transform weaponScaleRoot;
        public MMF_Player weaponFileFeedback;
        public MMF_Player unitInjuryFeedback;
        public MMF_Player unitPickupFeedback;
    }
}