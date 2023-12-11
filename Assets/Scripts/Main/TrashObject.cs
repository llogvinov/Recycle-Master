using DG.Tweening;
using ObjectsData;
using UnityEngine;

namespace Main
{
    public class TrashObject : MonoBehaviour
    {
        [SerializeField] private Vector3 _dragRotation;

        public TrashObjectSpawner TrashObjectSpawner { get; private set; }
        public TrashData TrashData { get; private set; }
        public bool IsDragged { get; private set; }
        public bool IsThrown { get; set; }
        
        private Rigidbody _rigidbody;
        private Collider _collider;

        private Tween _rotateTween;

        public void Init(TrashData trashData, TrashObjectSpawner trashObjectSpawner)
        {
            _rigidbody = GetComponentInChildren<Rigidbody>();
            _collider = GetComponentInChildren<Collider>();

            TrashObjectSpawner = trashObjectSpawner;
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

            RotateObject();
        }

        private void RotateObject()
        {
            _rotateTween = transform.DORotate(_dragRotation, 0.5f);
        }

        public void OnEndDrag()
        {
            _rotateTween?.Kill();

            IsDragged = false;
            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = false;
        }

        public void ToggleInteraction(bool enable) => 
            _collider.enabled = enable;
    }
}