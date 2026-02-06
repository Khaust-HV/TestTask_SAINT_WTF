using ProjectFiles.Scripts.Camera;
using ProjectFiles.Scripts.Characters;
using ProjectFiles.Scripts.Managers;

namespace ProjectFiles.Scripts.Installers
{
    public sealed class DependencyContainer // Simple DI
    {
        #region Controllers
        public IContolPlayer ControlPlayer { get; private set; }
        public IContolCamera ControlCamera { get; private set; }
        #endregion

        #region Managers
        public IContolGameplayInputState ControlInputState { get; private set; }
        #endregion

        public DependencyContainer(PlayerController playerController, CameraController cameraController)
        {
            // Set controllers
            ControlPlayer = playerController;
            ControlCamera = cameraController;

            // Set managers
            ControlInputState = new InputManager(this);
        }
    }
}