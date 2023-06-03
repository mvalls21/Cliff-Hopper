using UnityEngine;

    public class PausableRigidBody : MonoBehaviour
    {
        protected Rigidbody _rigidbody;

        private Vector3 _pausedVelocity;

        private Vector3 _pausedAngularVelocity;

        public void SetupPausableRigidBody()
        {
            GameManager.Instance.GamePausedChanged += OnPauseChanged;

            _rigidbody = GetComponent<Rigidbody>();
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
            }
            else
            {
                _rigidbody.isKinematic = false;
                _rigidbody.velocity = _pausedVelocity;
                _rigidbody.angularVelocity = _pausedAngularVelocity;
            }
        }
    }