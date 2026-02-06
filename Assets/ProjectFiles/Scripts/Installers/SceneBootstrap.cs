using ProjectFiles.Scripts.Camera;
using ProjectFiles.Scripts.Characters;
using UnityEngine;

namespace ProjectFiles.Scripts.Installers
{
    public sealed class SceneBootstrap : MonoBehaviour // Simple DI & Entry point
    {
        [Header("Dependencies")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private CameraController _cameraController;

        // Components
        private DependencyContainer _container;   

        private void Awake()
        {
            // Set components
            _container = GetDependencyContainer();

            // Injection
            DependencyInjection();

            // Scene initialization
            SceneInitialization();

            // Post scene initialization
            PostSceneInitialization();
        }

        private DependencyContainer GetDependencyContainer()
        {
            return new DependencyContainer(_playerController, _cameraController);
        }

        private void DependencyInjection()
        {
            foreach (var mono in FindObjectsOfType<MonoBehaviour>())
            {
                if (mono is IInjectable injectable)
                {
                    injectable.Construct(_container);
                }
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