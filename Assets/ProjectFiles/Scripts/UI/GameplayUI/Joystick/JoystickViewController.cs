using ProjectFiles.Scripts.Installers;
using UnityEngine;

namespace ProjectFiles.Scripts.UI.GameplayUI
{
    public sealed class JoystickViewController : MonoBehaviour, IInjectable, IControlJoystickView
    {
        [SerializeField] private RectTransform _canvasRect;
        [SerializeField] private RectTransform _joystickBackgroundRect;
        [SerializeField] private RectTransform _stickRect;

        // Properties
        public Vector2 StartPosition { get; set; }
        public Vector2 JoystickDirection { get; set; }
        public float JoystickStrength { get; set; }

        // Components
        private DependencyContainer _container;
        private CanvasGroup _canvasGroup;

        // Fields
        private float _maxJoystickRadius;

        public void Construct(DependencyContainer container)
        {
            // Set components
            _container = container;
            _canvasGroup = GetComponent<CanvasGroup>();

            // Set fields
            _maxJoystickRadius = _joystickBackgroundRect.sizeDelta.x * 0.5f;

            // Set initial state
            _canvasGroup.alpha = 0f;
        }

        public void SetJoystickActive(bool isActive)
        {
            _canvasGroup.alpha = isActive ? 1f : 0f;

            if (!isActive)
            {
                _stickRect.anchoredPosition = Vector2.zero;

                return;
            }

            Vector2 localPoint;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRect,
                StartPosition,
                null,
                out localPoint
            );

            _joystickBackgroundRect.anchoredPosition = localPoint;
            _stickRect.anchoredPosition = Vector2.zero;
        }

        public void UpdateJoystickDirection()
        {
            var stickOffset = JoystickDirection * (JoystickStrength * _maxJoystickRadius);

            if (stickOffset.magnitude > _maxJoystickRadius) stickOffset = stickOffset.normalized * _maxJoystickRadius;

            _stickRect.anchoredPosition = stickOffset;
        }
    }
}