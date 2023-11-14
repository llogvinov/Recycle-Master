using Main;
using UnityEngine;

namespace Core
{
    public class TouchInputSystem : MonoBehaviour
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
        private Vector3 _input;

        private void Awake()
        {
            _camera = Camera.main;
            _rayCheckerPlane.transform.position = Vector3.up * _objectFloatHeight;
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                _input = touch.position;
                var ray = _camera.ScreenPointToRay(_input);
                
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        OnTouchBegan(ray);
                        break;
                    case TouchPhase.Moved:
                        OnTouchMoved(ray);
                        break;
                    case TouchPhase.Ended:
                        OnTouchEnded();
                        break;
                }
            }
        }

        private void OnTouchBegan(Ray ray)
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

        private void OnTouchMoved(Ray ray)
        {
            if (Physics.Raycast(ray, out var hit, 50f, _rayCheckerLayerMask))
            {
                _dragged.transform.position =
                    Vector3.Lerp(_dragged.transform.position, hit.point, _speed * Time.deltaTime);
            }
        }

        private void OnTouchEnded()
        {
            if (_dragged != null)
            {
                _dragged.OnEndDrag();
                _dragged = null;
            }
        }
    }
}