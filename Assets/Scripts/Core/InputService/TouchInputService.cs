using UnityEngine;

namespace Core.InputService
{
    public class TouchInputService : InputService
    {
        private void Update()
        {
            if (Input.touchCount <= 0) return;
            
            var touch = Input.GetTouch(0);
            InputPosition = touch.position;
            var ray = Camera.ScreenPointToRay(InputPosition);
                
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnInputBegan(ray);
                    break;
                case TouchPhase.Moved:
                    OnInputMoved(ray);
                    break;
                case TouchPhase.Ended:
                    OnInputEnded();
                    break;
            }
        }
    }
}