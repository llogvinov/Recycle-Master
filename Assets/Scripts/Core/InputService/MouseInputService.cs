namespace Core.InputService
{
    public class MouseInputService : InputService
    {
        private void Update()
        {
            InputPosition = UnityEngine.Input.mousePosition;
            var ray = Camera.ScreenPointToRay(InputPosition);
            
            if (UnityEngine.Input.GetMouseButtonDown(0)) 
                OnInputBegan(ray);

            if (Dragged != null) 
                OnInputMoved(ray);

            if (UnityEngine.Input.GetMouseButtonUp(0)) 
                OnInputEnded();
        }
    }
}