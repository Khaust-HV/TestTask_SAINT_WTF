using UnityEngine;

namespace ProjectFiles.Scripts.Notification
{
    public interface IControlNotification
    {
        public void Show(Transform target, string message);
        public void Hide(Transform target);
    }
}