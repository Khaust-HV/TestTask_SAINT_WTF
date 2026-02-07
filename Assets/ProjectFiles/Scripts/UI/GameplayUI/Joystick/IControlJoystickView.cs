using UnityEngine;

namespace ProjectFiles.Scripts.UI.GameplayUI
{
    public interface IControlJoystickView
    {
        // Properties
        public Vector2 StartPosition { set; }
        public Vector2 JoystickDirection { set; }
        public float JoystickStrength { set; }

        // Control methods
        public void SetJoystickActive(bool isActive);
        public void UpdateJoystickDirection();
    }
}