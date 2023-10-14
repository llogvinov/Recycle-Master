using UnityEngine;

namespace Main
{
    public class TrashObject : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Collider _collider;

        private void Awake()
        {
            _rigidbody = GetComponentInChildren<Rigidbody>();
            _collider = GetComponentInChildren<Collider>();
        }

        public void OnStartDrag()
        {
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        public void OnEndDrag()
        {
            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = false;
        }
    }
}