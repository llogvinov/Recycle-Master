using System;
using Main;
using UnityEngine;

namespace Core.InputService
{
    public class InputService : MonoBehaviour, IService
    {
        public static Action<TrashObject, TrashCan> OnRight;
        public static Action<TrashObject> OnWrong;
        
        [SerializeField] protected LayerMask _interactableLayerMask;
        [SerializeField] protected LayerMask _trashCanLayerMask;
        
        protected Camera Camera;
        protected TrashObject Selected;
        protected Vector3 InputPosition;
        
        protected void Awake() => 
            Camera = Camera.main;

        protected void OnInputBegan(Ray ray)
        {
            if (!Physics.Raycast(ray, out var hit, 50f, _trashCanLayerMask.value)) return;
            if (!hit.collider.transform.parent.TryGetComponent<TrashCan>(out var trashCan)) return;

            TrashCanSpawner.SelectTrashCan(trashCan);
        }

        protected void OnHold(Ray ray)
        {
            if (!Physics.Raycast(ray, out var hit, 50f, _interactableLayerMask.value)) return;
            if (!hit.collider.transform.parent.TryGetComponent<TrashObject>(out var trashObject)) return; 
                
            Selected = trashObject;
        }

        protected void OnInputEnded()
        {
            if (Selected == null) return;

            if (Selected.TrashData.Type == RecycleController.TrashCan.TrashCanData.Type)
            {
                Debug.Log("right");
                OnRight?.Invoke(Selected, RecycleController.TrashCan);
                //RecycleController.DisposeAnimation(Selected, RecycleController.TrashCan);
            }
            else
            {
                Debug.Log("wrong");
                OnWrong?.Invoke(Selected);
            }
            Selected = null;
        }
    }
}