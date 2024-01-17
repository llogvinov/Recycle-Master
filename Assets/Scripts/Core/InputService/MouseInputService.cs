using UnityEngine;

namespace Core.InputService
{
    public class MouseInputService : InputService
    {
        private void Update()
        {
            InputPosition = Input.mousePosition;
            var ray = Camera.ScreenPointToRay(InputPosition);
            
            if (Input.GetMouseButton(0))
                OnMouseButtonHeld(ray);

            if (Input.GetMouseButtonDown(0))
                OnMouseButtonDown(ray);

            if (Input.GetMouseButtonUp(0)) 
                OnReleaseButton();
        }
    }
}