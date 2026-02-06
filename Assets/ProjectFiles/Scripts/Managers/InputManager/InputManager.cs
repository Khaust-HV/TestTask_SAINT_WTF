using ProjectFiles.Scripts.Installers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectFiles.Scripts.Managers
{
    public sealed class InputManager : IContolGameplayInputState // Also acts as an input hendler
    {
        // Components
        private DependencyContainer _container;
        private InputMap _inputMap;

        // Move character fields
        private bool _isScreenPressActive;
        private const float MIN_JOYSTICK_RADIUS = 20f;
        private const float MAX_JOYSTICK_RADIUS = 120f;
        private Vector2 _joystickCenter;

        public InputManager(DependencyContainer container)
        {
            // Set components
            _container = container;
            _inputMap = new InputMap();

            _inputMap.Enable();
        }

        public void SetGameplayInputActive(bool isActive)
        {
            if (isActive)
            {
                _inputMap.Player.ScreenPress.started += OnScreenPressEnabled;
                _inputMap.Player.ScreenPress.canceled += OnScreenPressDisabled;

                _inputMap.Player.PositionScreenPress.performed += OnMovementAcrossTheScreen;
            }
            else
            {
                _inputMap.Player.ScreenPress.started -= OnScreenPressEnabled;
                _inputMap.Player.ScreenPress.canceled -= OnScreenPressDisabled;
                
                _inputMap.Player.PositionScreenPress.performed -= OnMovementAcrossTheScreen;
            }
        }

        #region Gameplay input
        private void OnScreenPressEnabled(InputAction.CallbackContext context)
        {
            _joystickCenter = _inputMap.Player.PositionScreenPress.ReadValue<Vector2>();

            _isScreenPressActive = true;
        }

        private void OnScreenPressDisabled(InputAction.CallbackContext context)
        {
            _container.ControlPlayer.IsMoveCharacterActive = false;

            _isScreenPressActive = false;
        }

        private void OnMovementAcrossTheScreen(InputAction.CallbackContext context)
        {
            Vector2 currentTouchPosition = context.ReadValue<Vector2>();

            Vector2 delta = currentTouchPosition - _joystickCenter;

            float distance = delta.magnitude;
            Vector2 direction = delta.normalized;

            if (distance < MIN_JOYSTICK_RADIUS || !_isScreenPressActive)
            {
                _container.ControlPlayer.IsMoveCharacterActive = false;

                return;
            }

            if (distance > MAX_JOYSTICK_RADIUS) delta = direction * MAX_JOYSTICK_RADIUS;

            _container.ControlPlayer.IsMoveCharacterActive = true;

            direction = delta / MAX_JOYSTICK_RADIUS;

            _container.ControlPlayer.MoveDirection = new Vector3(direction.x, 0, direction.y);

            Debug.Log($"Joystick MOVE: {delta}, Normalized: {delta / MAX_JOYSTICK_RADIUS}");
        } 
        #endregion
    }
}