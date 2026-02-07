using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ProjectFiles.Scripts.Notification
{
    public sealed class NotificationViewController : MonoBehaviour // Create by AI
    {
        private RectTransform _root;
        private TMP_Text _text;
        private Image _background;

        private Transform _target;
        private UnityEngine.Camera _camera;
        private bool _isVisible;

        private Vector3 _offset = new Vector3(0f, 1.5f, 0f);

        public Transform Target => _target;
        public bool IsBusy => _target != null;

        public void Construct()
        {
            _camera = UnityEngine.Camera.main;
            CreateUI();
            HideImmediate();
        }

        private void LateUpdate()
        {
            if (!_isVisible || _target == null)
                return;

            UpdatePosition();
        }

        public void Setup(Transform target, string message)
        {
            _target = target;
            _text.text = message;
        }

        public void Show()
        {
            _isVisible = true;
            _root.gameObject.SetActive(true);
            UpdatePosition();
        }

        public void Hide()
        {
            _isVisible = false;
            _target = null;
            _root.gameObject.SetActive(false);
        }

        public void HideImmediate()
        {
            _isVisible = false;
            _target = null;
            _root.gameObject.SetActive(false);
        }

        private void UpdatePosition()
        {
            Vector3 worldPos = _target.position + _offset;
            Vector3 screenPos = _camera.WorldToScreenPoint(worldPos);

            if (screenPos.z < 0f)
            {
                _root.gameObject.SetActive(false);
                return;
            }

            _root.position = screenPos;
        }

        private void CreateUI()
        {
            _root = GetComponent<RectTransform>();
            _root.pivot = new Vector2(0.5f, 0f);

            // ===== Bubble Root =====
            var bubbleGO = new GameObject("Bubble");
            bubbleGO.transform.SetParent(transform, false);

            var bubbleRect = bubbleGO.AddComponent<RectTransform>();
            bubbleRect.pivot = new Vector2(0.5f, 0f);
            bubbleRect.anchorMin = new Vector2(0.5f, 0f);
            bubbleRect.anchorMax = new Vector2(0.5f, 0f);

            // ===== Shadow =====
            var shadowGO = new GameObject("Shadow");
            shadowGO.transform.SetParent(bubbleGO.transform, false);

            var shadow = shadowGO.AddComponent<Image>();
            shadow.color = new Color(0f, 0f, 0f, 0.35f);

            var shadowRect = shadowGO.GetComponent<RectTransform>();
            shadowRect.sizeDelta = new Vector2(280f, 64f);
            shadowRect.anchoredPosition = new Vector2(0f, -4f);

            // ===== Background =====
            var bgGO = new GameObject("Background");
            bgGO.transform.SetParent(bubbleGO.transform, false);

            _background = bgGO.AddComponent<Image>();
            _background.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

            var bgRect = bgGO.GetComponent<RectTransform>();
            bgRect.sizeDelta = new Vector2(280f, 64f);

            // ===== Content =====
            var contentGO = new GameObject("Content");
            contentGO.transform.SetParent(bgGO.transform, false);

            var contentRect = contentGO.AddComponent<RectTransform>();
            contentRect.anchorMin = Vector2.zero;
            contentRect.anchorMax = Vector2.one;
            contentRect.offsetMin = new Vector2(14f, 10f);
            contentRect.offsetMax = new Vector2(-14f, -10f);

            // ===== Text =====
            var textGO = new GameObject("Text");
            textGO.transform.SetParent(contentGO.transform, false);

            _text = textGO.AddComponent<TextMeshProUGUI>();
            _text.fontSize = 24;
            _text.color = Color.white;
            _text.alignment = TextAlignmentOptions.Center;
            _text.enableAutoSizing = true;
            _text.fontSizeMin = 18;
            _text.fontSizeMax = 26;

            var textRect = textGO.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
        }
    }
}