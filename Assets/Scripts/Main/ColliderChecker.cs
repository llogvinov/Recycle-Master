using System;
using UnityEngine;

namespace Main
{
    [RequireComponent(typeof(Collider))]
    public class ColliderChecker : MonoBehaviour
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
            if (other.TryGetComponent<TrashObject>(out var trashObject))
            {
                if (trashObject.IsDragged || trashObject.IsThrown) return;
                
                trashObject.IsThrown = true;
                if (trashObject.TrashData.Type == _trashCan.TrashCanData.Type)
                {
                    Success?.Invoke(trashObject, _trashCan);
                    Debug.Log("success");
                }
                else
                {
                    Fail?.Invoke(trashObject);
                    Debug.Log("fail");
                }
            }
        }
    }
}