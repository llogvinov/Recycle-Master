using ObjectsData;
using UnityEngine;

namespace Main
{
    public class TrashObject : MonoBehaviour
    {
        public TrashData TrashData { get; private set; }
        public bool IsDragged { get; private set; }
        public bool IsThrown { get; set; }
        
        private Rigidbody _rigidbody;
        private Collider _collider;

        public void Init(TrashData trashData)
        {
            _rigidbody = GetComponentInChildren<Rigidbody>();
            _collider = GetComponentInChildren<Collider>();
            
            TrashData = trashData;
            IsDragged = false;
            IsThrown = false;
        }

        public void OnStartDrag()
        {
            IsDragged = true;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
        }

        public void OnEndDrag()
        {
            IsDragged = false;
            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = false;
        }

        public void ToggleInteraction(bool enable) => 
            _collider.enabled = enable;
    }
}