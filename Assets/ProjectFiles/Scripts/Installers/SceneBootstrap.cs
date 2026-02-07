using ProjectFiles.Scripts.Camera;
using ProjectFiles.Scripts.Characters;
using ProjectFiles.Scripts.UI.GameplayUI;
using UnityEngine;

namespace ProjectFiles.Scripts.Installers
{
    public sealed class SceneBootstrap : MonoBehaviour // Simple DI & Entry point
    {
        [Header("UI controllers")]
        [SerializeField] private JoystickViewController _joystickViewController;
        [Space(10f)]

        [Header("Other controllers")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private CameraController _cameraController;

        // Components
        private DependencyContainer _container;   

        private void Awake()
        {
            // Create dependency container
            CreateDependencyContainer();

            // Injection
            DependencyInjection();

            // Scene initialization
            SceneInitialization();

            // Post scene initialization
            PostSceneInitialization();
        }

        private void CreateDependencyContainer()
        {
            _container =  new DependencyContainer(
                _playerController, 
                _cameraController,
                _joystickViewController
            );
        }

        private void DependencyInjection()
        {
            foreach (var mono in FindObjectsOfType<MonoBehaviour>())
            {
                if (mono is IInjectable injectable) injectable.Construct(_container);
            }

            Debug.Log("[SceneBootstrap] Dependencies injection COMPLETED");
        }

        private void SceneInitialization()
        {
            Debug.Log("[SceneBootstrap] Scene initialization COMPLETED");
        }

        private void PostSceneInitialization()
        {
            _container.ControlInputState.SetGameplayInputActive(true);

            Debug.Log("[SceneBootstrap] Post scene initialization COMPLETED");
        }
    }
}