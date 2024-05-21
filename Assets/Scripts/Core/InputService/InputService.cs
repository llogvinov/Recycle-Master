using System;
using Main;
using Main.Level;
using UnityEngine;

namespace Core.InputService
{
    public class InputService : MonoBehaviour, IService
    {
        public static Action<TrashObject> ObjectSelected;
        
        [SerializeField] protected LayerMask _interactableLayerMask;
        [SerializeField] protected LayerMask _trashCanLayerMask;
        
        protected Camera Camera;
        protected TrashObject Selected;
        protected Vector3 InputPosition;
        
        protected void Awake()
        {
            Camera = Camera.main;
            ObjectSelectHandler objectSelectHandler = new();
        }

        protected void OnInputBegan(Ray ray)
        {
            if (!Physics.Raycast(ray, out var hit, 50f, _trashCanLayerMask.value)) return;
            if (!hit.collider.transform.parent.TryGetComponent<TrashCan>(out var trashCan)) return;
            if (!trashCan.IsInteractable) return;
            
            TrashCanSpawner.SelectTrashCan(trashCan);
        }

        protected void OnHold(Ray ray)
        {
            if (!Physics.Raycast(ray, out var hit, 50f, _interactableLayerMask.value)) return;
            if (!hit.collider.transform.parent.TryGetComponent<TrashObject>(out var trashObject)) return; 
            if (!trashObject.IsInteractable) return;
                
            Selected = trashObject;
        }

        protected void OnInputEnded()
        {
            if (Selected == null) return;

            ObjectSelected?.Invoke(Selected);
            Selected = null;
        }
    }
}