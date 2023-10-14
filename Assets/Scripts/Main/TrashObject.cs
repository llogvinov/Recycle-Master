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
            Debug.Log("on start");
        }

        public void OnEndDrag()
        {
            Debug.Log("on end");
        }
    }
}