using UnityEngine;

namespace Game.Framework
{
    public class InputManager : MonoSingleton<InputManager>
    { 
        private Vector2 _inputAxis = Vector2.zero;
        private Vector2 _aimAxis = Vector2.zero;
        private bool _isShootButtonDown = false;
        
        private int _selectedWeaponIndex = 0;
        
        public Vector2 GetInputAxis()
        {
            return _inputAxis;
        }

        public void SetInputAxis(Vector2 vector2)
        {
            _inputAxis = vector2;
        }
        
        public float GetAimRotation()
        {
            var dir = _aimAxis.normalized;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            return angle;
        }
        
        public void SetAimAxis(Vector2 vector2)
        {
            _aimAxis = vector2;
        }

        public bool GetShootButtonDown()
        {
            return _isShootButtonDown;
        }
        
        public void TriggerShootButtonDown()
        {
            _isShootButtonDown = true;
        }
        
        public void ClearShootButtonDown()
        {
            _isShootButtonDown = false;
        }

        public int GetSelectedWeaponIndex()
        {
            return _selectedWeaponIndex;
        }
        
        public void SetSelectedWeaponIndex(int index)
        {
            _selectedWeaponIndex = index;
        }
    }
}