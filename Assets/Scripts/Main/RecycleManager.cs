namespace Main
{
    public class RecycleManager
    {
        private void Start()
        {
            ColliderChecker.Success += OnSuccess;
            ColliderChecker.Fail += OnFail;
        }

        private void OnDestroy()
        {
            ColliderChecker.Success -= OnSuccess;
            ColliderChecker.Fail -= OnFail;
        }

        private void OnSuccess(TrashObject trashObject, TrashCan trashCan)
        {
            
        }

        private void OnFail(TrashObject obj)
        {
            
        }
    }
}