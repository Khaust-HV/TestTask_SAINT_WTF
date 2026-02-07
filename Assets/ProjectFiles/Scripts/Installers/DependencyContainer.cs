using ProjectFiles.Scripts.Camera;
using ProjectFiles.Scripts.Characters;
using ProjectFiles.Scripts.Managers;
using ProjectFiles.Scripts.UI.GameplayUI;

namespace ProjectFiles.Scripts.Installers
{
    public sealed class DependencyContainer // Simple DI (class G)
    {
        #region UI controllers
        public IControlJoystickView ControlJoystickView { get; private set; }
        #endregion

        #region Other controllers
        public IContolPlayer ControlPlayer { get; private set; }
        public IContolCamera ControlCamera { get; private set; }
        #endregion

        #region Managers
        public IContolGameplayInputState ControlInputState { get; private set; }
        #endregion

        public DependencyContainer(
            PlayerController playerController, 
            CameraController cameraController,
            JoystickViewController joystickViewController
        )
        {
            // Set UI controllers
            ControlJoystickView = joystickViewController;

            // Set other controllers
            ControlPlayer = playerController;
            ControlCamera = cameraController;

            // Set managers
            ControlInputState = new InputManager(this);
        }
    }
}