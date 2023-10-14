using System;
using Main;
using UnityEngine;

namespace Core
{
    public class InputSystem : MonoBehaviour
    {
        [SerializeField] private float _objectLiftHeight = 1f;
        [SerializeField] private float _speed;

        private Camera _camera;
        [SerializeField] private Transform _dragged;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var temp = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20f));
                temp.y = _objectLiftHeight;
                _dragged.position = Vector3.Lerp(_dragged.position, temp, _speed * Time.deltaTime);
            }
            
            /*Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                _dragged.position = hit.point + Vector3.up * _objectLiftHeight;
            }*/

            /*if (Input.GetMouseButtonDown(0))
            {
                OnMousePressed();
            }

            if (Input.GetMouseButtonUp(0) && _dragged != null)
            {
                //_dragged.OnEndDrag();
                _dragged = null;
            }

            if (_dragged != null)
            {
                Vector3 newPosition = GetMousePosition();
                newPosition.y = _objectLiftHeight;
                _dragged.transform.position = newPosition;
            }*/
        }

        private void OnMousePressed()
        {
            Ray ray = _camera.ScreenPointToRay(GetMousePosition());
            Debug.DrawLine(_camera.transform.position, GetMousePosition(), Color.red, 1f, false);
            if (Physics.Raycast(ray, out var hit, 50f))
            {
                Debug.Log(hit.distance);
                if (hit.collider != null)
                {
                    _dragged = hit.transform;
                    //_dragged.OnStartDrag();
                }
            }
        }

        private Vector3 GetMousePosition() => 
            GetPositionFromInput(Input.mousePosition);

        private Vector3 GetPositionFromInput(Vector3 input)
        {
            var x = input.x;
            var y = input.y;

            var point = _camera.ScreenToWorldPoint(new Vector3(x, y, 13f));
            Debug.Log(point);
            return point;
        }
    }
}