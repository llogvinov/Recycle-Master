using Main;
using UnityEngine;

namespace Core
{
    public class InputSystem : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [Space]
        [SerializeField] private LayerMask _interactableLayerMask;
        [SerializeField] private LayerMask _rayCheckerLayerMask;
        [Space]
        [SerializeField] private GameObject _rayCheckerPlane;
        
        [Range(1f, 3f)]
        [SerializeField] private float _objectFloatHeight;
        

        private Camera _camera;
        private TrashObject _dragged;

        private void Awake()
        {
            _camera = Camera.main;
            _rayCheckerPlane.transform.position = Vector3.up * _objectFloatHeight;
        }

        private void Update()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out var hit, 50f, _interactableLayerMask))
                {
                    if (hit.collider.transform.parent.TryGetComponent<TrashObject>(out var trashObject))
                    {
                        _dragged = trashObject;
                        _dragged.OnStartDrag();
                    }
                }
            }

            if (Input.GetMouseButtonUp(0) && _dragged != null)
            {
                _dragged.OnEndDrag();
                _dragged = null;
            }
            
            if (_dragged != null)
            {
                if (Physics.Raycast(ray, out var hit, 50f, _rayCheckerLayerMask))
                {
                    _dragged.transform.position = 
                        Vector3.Lerp(_dragged.transform.position, hit.point, _speed * Time.deltaTime);
                }
            }
        }
    }
}