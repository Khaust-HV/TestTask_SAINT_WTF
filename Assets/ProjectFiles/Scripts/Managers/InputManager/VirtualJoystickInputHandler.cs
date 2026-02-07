using ProjectFiles.Scripts.Installers;
using UnityEngine;

namespace ProjectFiles.Scripts.Managers
{
    public sealed class VirtualJoystickInputHandler
    {
        // Properties
        public Vector2 StartPosition { get; set; }

        // Components
        private DependencyContainer _container;

        // Base fields
        private bool _isJoystickActive;

        // Move fields
        private const byte MIN_JOYSTICK_RADIUS = 25;
        private const byte MAX_JOYSTICK_RADIUS = 250;

        public VirtualJoystickInputHandler(DependencyContainer container)
        {
            // Set components
            _container = container;
        }

        public void SetJoystickActive(bool isActive)
        {
            if (isActive)
            {
                // Joystick view enable
                _container.ControlJoystickView.StartPosition = StartPosition;

                _container.ControlJoystickView.SetJoystickActive(true);
            }
            else
            {
                // Joystick view disable
                _container.ControlJoystickView.SetJoystickActive(false);

                _container.ControlJoystickView.JoystickDirection = Vector2.zero;

                _container.ControlJoystickView.JoystickStrength = 0f;

                // Player move disable
                _container.ControlPlayer.MoveDirection = Vector3.zero;

                _container.ControlPlayer.MoveSpeedScale = 0f;

                _container.ControlPlayer.IsMoveCharacterActive = false;
            }

            _isJoystickActive = isActive;
        }

        public void UpdateJoystickDirection(Vector2 newScreenPressPosition)
        {
            if (!_isJoystickActive) return;

            // Calculate joystick direction
            var offset = newScreenPressPosition - StartPosition;
            var distance = offset.magnitude;

            var joystickDirection = offset.normalized;

            var joystickStrength = Mathf.InverseLerp(
                MIN_JOYSTICK_RADIUS,
                MAX_JOYSTICK_RADIUS,
                Mathf.Min(distance, MAX_JOYSTICK_RADIUS)
            );

            // Joystick view move
            _container.ControlJoystickView.JoystickDirection = joystickDirection;

            _container.ControlJoystickView.JoystickStrength = joystickStrength;

            _container.ControlJoystickView.UpdateJoystickDirection();

            // Player move
            if (distance < MIN_JOYSTICK_RADIUS)
            {
                _container.ControlPlayer.MoveDirection = Vector3.zero;

                _container.ControlPlayer.MoveSpeedScale = 0f;

                _container.ControlPlayer.IsMoveCharacterActive = false;

                return;
            }
            else _container.ControlPlayer.IsMoveCharacterActive = true;

            _container.ControlPlayer.MoveDirection = new Vector3(joystickDirection.x, 0, joystickDirection.y);

            _container.ControlPlayer.MoveSpeedScale = joystickStrength;
        }
    }
}