using UnityEngine;

    public class PausableRigidBody : MonoBehaviour
    {
        protected Rigidbody _rigidbody;

        protected Animator _animator;

        private Vector3 _pausedVelocity;

        private Vector3 _pausedAngularVelocity;

        public void SetupPausableRigidBody()
        {
            GameManager.Instance.GamePausedChanged += OnPauseChanged;

            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _pausedVelocity = _rigidbody.velocity;
            _pausedAngularVelocity = _rigidbody.angularVelocity;
        }

        public void OnDestroy()
        {
            GameManager.Instance.GamePausedChanged -= OnPauseChanged;
        }

        private void OnPauseChanged(object source, bool paused)
        {
            if (paused)
            {
                _pausedVelocity = _rigidbody.velocity;
                _pausedAngularVelocity = _rigidbody.angularVelocity;
                _rigidbody.isKinematic = true;

                if (_animator != null)
                    _animator.speed = 0;
            }
            else
            {
                _rigidbody.isKinematic = false;
                _rigidbody.velocity = _pausedVelocity;
                _rigidbody.angularVelocity = _pausedAngularVelocity;

                if (_animator != null)
                    _animator.speed = 1;
            }
        }
    }