using ProjectFiles.Scripts.Installers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectFiles.Scripts.Managers
{
    public sealed class InputManager : IContolGameplayInputState
    {
        // Components
        private DependencyContainer _container;
        private InputMap _inputMap;
        private VirtualJoystickInputHandler _joystick;

        public InputManager(DependencyContainer container)
        {
            // Set components
            _container = container;
            _inputMap = new InputMap();
            _joystick = new VirtualJoystickInputHandler(container);

            _inputMap.Enable();
        }

        public void SetGameplayInputActive(bool isActive)
        {
            if (isActive)
            {
                _inputMap.Player.TouchPress.started += OnScreenPressEnabled;
                _inputMap.Player.TouchPress.canceled += OnScreenPressDisabled;

                _inputMap.Player.TouchPressPosition.performed += OnMovementAcrossTheScreen;
            }
            else
            {
                _inputMap.Player.TouchPress.started -= OnScreenPressEnabled;
                _inputMap.Player.TouchPress.canceled -= OnScreenPressDisabled;
                
                _inputMap.Player.TouchPressPosition.performed -= OnMovementAcrossTheScreen;
            }
        }

        #region Gameplay input
        private void OnScreenPressEnabled(InputAction.CallbackContext context)
        {
            if (Input.touchCount < 1) return;

            _joystick.StartPosition = Input.GetTouch(0).position; // Temporary solution

            _joystick.SetJoystickActive(true);
        }

        private void OnScreenPressDisabled(InputAction.CallbackContext context)
        {
            _joystick.SetJoystickActive(false);
        }

        private void OnMovementAcrossTheScreen(InputAction.CallbackContext context)
        {
            if (Input.touchCount < 1) return;

            _joystick.UpdateJoystickDirection(Input.GetTouch(0).position); // Temporary solution
        } 
        #endregion
    }
}