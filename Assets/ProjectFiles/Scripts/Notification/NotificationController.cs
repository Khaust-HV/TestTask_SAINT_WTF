using System.Collections.Generic;
using ProjectFiles.Scripts.Installers;
using UnityEngine;

namespace ProjectFiles.Scripts.Notification
{
    public sealed class NotificationController : MonoBehaviour, IInjectable, IControlNotification
    {
        [Header("Base settings")]
        [SerializeField] private Canvas _popupCanvas; // Temporary solution
        [SerializeField] private int _poolSize;

        // Components
        private DependencyContainer _container;

        // Fields
        private readonly List<NotificationViewController> _notificationViews = new();
        private int _currentIndex;

        public void Construct(DependencyContainer container)
        {
            // Set components
            _container = container;

            // Create pool
            for (int i = 0; i < _poolSize; i++)
            {
                var view = Instantiate(_container.NotificationViewPrefab, _popupCanvas.transform);

                view.Construct();

                view.HideImmediate();

                _notificationViews.Add(view);
            }
        }

        public void Show(Transform target, string message)
        {
            foreach (var view in _notificationViews)
            {
                if (view.Target == target)
                {
                    view.Setup(target, message);

                    view.Show();

                    return;
                }
            }

            foreach (var view in _notificationViews)
            {
                if (!view.IsBusy)
                {
                    view.Setup(target, message);

                    view.Show();

                    return;
                }
            }

            var nextView = _notificationViews[_currentIndex];

            _currentIndex = (_currentIndex + 1) % _notificationViews.Count;

            nextView.HideImmediate();

            nextView.Setup(target, message);

            nextView.Show();
        }

        public void Hide(Transform target)
        {
            foreach (var view in _notificationViews)
            {
                if (view.Target == target)
                {
                    view.Hide();

                    return;
                }
            }
        }
    }
}