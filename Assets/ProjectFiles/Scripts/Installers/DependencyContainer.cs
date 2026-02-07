using ProjectFiles.Scripts.Buildings.Resources;
using ProjectFiles.Scripts.Camera;
using ProjectFiles.Scripts.Characters.Player;
using ProjectFiles.Scripts.Managers;
using ProjectFiles.Scripts.Notification;
using ProjectFiles.Scripts.UI.GameplayUI;

namespace ProjectFiles.Scripts.Installers
{
    public sealed class DependencyContainer // Simple DI (class G)
    {
        #region UI controllers
        public IControlJoystickView ControlJoystickView { get; private set; }
        #endregion

        #region Other controllers
        public IControlPlayer ControlPlayer { get; private set; }
        public IContolPlayerInventory ControlPlayerInventory { get; private set; }
        public IContolCamera ControlCamera { get; private set; }
        public IControlNotification ControlNotification { get; private set; }
        #endregion

        #region Prefabs
        // Temporary replacement of configuration files
        public ResourceController N1Prefab { get; private set; }
        public ResourceController N2Prefab { get; private set; }
        public ResourceController N3Prefab { get; private set; }
        public NotificationViewController NotificationViewPrefab{ get; private set; }
        #endregion

        #region Managers
        public IContolGameplayInputState ControlInputState { get; private set; }
        #endregion

        public DependencyContainer(
            ResourceController n1Prefab,
            ResourceController n2Prefab,
            ResourceController n3Prefab,
            NotificationViewController notificationViewPrefab,
            PlayerController playerController, 
            PlayerInventoryController playerInventoryController,
            CameraController cameraController,
            JoystickViewController joystickViewController,
            NotificationController notificationController
        )
        {
            // Set prefabs
            N1Prefab = n1Prefab;
            N2Prefab = n2Prefab;
            N3Prefab = n3Prefab;
            NotificationViewPrefab = notificationViewPrefab;

            // Set UI controllers
            ControlJoystickView = joystickViewController;

            // Set other controllers
            ControlPlayer = playerController;
            ControlPlayerInventory = playerInventoryController;
            ControlCamera = cameraController;
            ControlNotification = notificationController;

            // Set managers
            ControlInputState = new InputManager(this);
        }
    }
}