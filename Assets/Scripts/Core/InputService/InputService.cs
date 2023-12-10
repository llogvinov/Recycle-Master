using Main;
using UnityEngine;

namespace Core.InputService
{
    public class InputService : MonoBehaviour, IService
    {
        [SerializeField] protected float _speed;
        [Space]
        [SerializeField] protected LayerMask _interactableLayerMask;
        [SerializeField] protected LayerMask _rayCheckerLayerMask;
        [Space]
        [SerializeField] protected GameObject _rayCheckerPlane;
        [Range(1f, 3f)]
        [SerializeField] protected float _objectFloatHeight;
        
        protected Camera Camera;
        protected TrashObject Dragged;
        protected Vector3 InputPosition;
        
        protected void Awake()
        {
            Camera = Camera.main;
            _rayCheckerPlane.transform.position = Vector3.up * _objectFloatHeight;
        }
        
        protected void OnInputBegan(Ray ray)
        {
            if (!Physics.Raycast(ray, out var hit, 50f, _interactableLayerMask.value)) return;
            if (!hit.collider.transform.parent.TryGetComponent<TrashObject>(out var trashObject)) return;
            
            Dragged = trashObject;
            Dragged.OnStartDrag();
        }

        protected void OnInputMoved(Ray ray)
        {
            if (Physics.Raycast(ray, out var hit, 50f, _rayCheckerLayerMask.value))
            {
                Dragged.transform.position =
                    Vector3.Lerp(Dragged.transform.position, hit.point, _speed * Time.deltaTime);
            }
        }

        protected void OnInputEnded()
        {
            if (Dragged == null) return;
            
            Dragged.OnEndDrag();
            Dragged = null;
        }
    }
}