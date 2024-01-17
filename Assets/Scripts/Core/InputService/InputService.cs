using Main;
using UnityEngine;

namespace Core.InputService
{
    public class InputService : MonoBehaviour, IService
    {
        [SerializeField] protected LayerMask _interactableLayerMask;
        [SerializeField] protected LayerMask _trashCanLayerMask;
        
        protected Camera Camera;
        protected TrashObject Selected;
        protected Vector3 InputPosition;
        
        protected void Awake() => 
            Camera = Camera.main;

        protected void OnMouseButtonDown(Ray ray)
        {
            if (!Physics.Raycast(ray, out var hit, 50f, _trashCanLayerMask.value)) return;
            if (!hit.collider.transform.parent.TryGetComponent<TrashCan>(out var trashCan)) return;

            TrashCanSpawner.SelectTrashCan(trashCan);
        }

        protected void OnMouseButtonHeld(Ray ray)
        {
            if (!Physics.Raycast(ray, out var hit, 50f, _interactableLayerMask.value)) return;
            if (!hit.collider.transform.parent.TryGetComponent<TrashObject>(out var trashObject)) return; 
                
            Selected = trashObject;
        }

        protected void OnReleaseButton()
        {
            if (Selected == null) return;
            
            RecycleController.DisposeAnimation(Selected, RecycleController.TrashCan);
            Selected = null;
        }
    }
}