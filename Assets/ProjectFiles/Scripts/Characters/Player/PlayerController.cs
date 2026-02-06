using ProjectFiles.Scripts.Installers;
using UnityEngine;

namespace ProjectFiles.Scripts.Characters
{
    public sealed class PlayerController : MonoBehaviour, IInjectable, IContolPlayer
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotateSpeed;

        // Properties
        public bool IsMoveCharacterActive {
            get 
            {
                return _isMoveCharacterActive;
            }
            set 
            {
                _isMoveCharacterActive = value;
                
                if (!_isMoveCharacterActive)
                {
                    // Freeze player
                    _rigidbody.velocity = Vector3.zero;
                    _rigidbody.angularVelocity = Vector3.zero;
                }
            }
        }
        public Vector3 MoveDirection { get; set; }

        // Components
        private DependencyContainer _container;
        private Rigidbody _rigidbody;

        // Move fields
        private bool _isMoveCharacterActive;
        private Vector3 _oldMoveDirection;

        public void Construct(DependencyContainer container)
        {
            // Set components
            _container = container;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (IsMoveCharacterActive) MoveCharacter();
        }

        #region Movement
        private void MoveCharacter()
        {
            if (MoveDirection.sqrMagnitude < 0.0001f) return;

            // Move
            _oldMoveDirection = Vector3.Lerp(_oldMoveDirection, MoveDirection, _rotateSpeed * Time.fixedDeltaTime);

            _oldMoveDirection.Normalize();

            var targetPosition = _rigidbody.position + _oldMoveDirection * _moveSpeed * Time.fixedDeltaTime;

            _rigidbody.MovePosition(targetPosition);

            // Rotation
            var targetRotation = Quaternion.LookRotation(_oldMoveDirection, Vector3.up);

            var smoothRotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, _rotateSpeed * Time.fixedDeltaTime);

            _rigidbody.MoveRotation(smoothRotation);
        }
        #endregion
    }
}