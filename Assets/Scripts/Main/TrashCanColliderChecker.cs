using System;
using UnityEngine;

namespace Main
{
    [RequireComponent(typeof(Collider))]
    public class TrashCanColliderChecker : MonoBehaviour
    {
        public static Action<TrashObject, TrashCan> Success;
        public static Action<TrashObject> Fail;

        private TrashCan _trashCan;
        
        public void Init(TrashCan trashCan)
        {
            _trashCan = trashCan;
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (!other.TryGetComponent<TrashObject>(out var trashObject)) return;
            if (trashObject.IsDragged || trashObject.IsThrown) return;
                
            OnTrashObjectThrown(trashObject);
        }

        private void OnTrashObjectThrown(TrashObject trashObject)
        {
            trashObject.IsThrown = true;
            trashObject.ToggleInteraction(false);
            if (trashObject.TrashData.Type == _trashCan.TrashCanData.Type)
            {
                Success?.Invoke(trashObject, _trashCan);
            }
            else
            {
                Fail?.Invoke(trashObject);
            }
        }
    }
}