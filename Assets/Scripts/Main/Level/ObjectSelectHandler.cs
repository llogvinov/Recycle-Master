using System;
using Core.InputService;

namespace Main.Level
{
    public class ObjectSelectHandler
    {
        public static Action<TrashObject, TrashCan> OnCorrectSelected;
        public static Action<TrashObject> OnWrongSelected;
        
        public ObjectSelectHandler() => 
            InputService.ObjectSelected += HandleSelectedObject;

        private void HandleSelectedObject(TrashObject trashObject)
        {
            if (trashObject.TrashData.Type == RecycleController.SelectedTrashCan.TrashCanData.Type)
                OnCorrectSelected?.Invoke(trashObject, RecycleController.SelectedTrashCan);
            else
                OnWrongSelected?.Invoke(trashObject);
        }
    }
}