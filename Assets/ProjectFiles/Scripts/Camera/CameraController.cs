using ProjectFiles.Scripts.Installers;
using UnityEngine;

namespace ProjectFiles.Scripts.Camera
{
    public sealed class CameraController : MonoBehaviour, IInjectable, IContolCamera
    {
        // Properties
        public UnityEngine.Camera Camera => _camera;

        // Components
        private DependencyContainer _container;
        private UnityEngine.Camera _camera;

        public void Construct(DependencyContainer container)
        {
            // Set components
            _container = container;
            _camera = GetComponent<UnityEngine.Camera>();
        }
    }
}