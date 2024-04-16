using ObjectsData;
using UnityEngine;
using UnityEngine.Events;

namespace Main
{
    public class TrashCan : MonoBehaviour
    {
        [HideInInspector]
        public UnityEvent OnSelected;
        
        [SerializeField] private GameObject _mesh;
        [Space] [Header("Animation")]
        [SerializeField] private Transform _objectStartPoint;
        [SerializeField] private Transform _objectEndPoint;
        [SerializeField] private ParticleSystem _fxDust;

        public TrashCanData TrashCanData { get; private set; }
        public bool IsInteractable { get; set; }
        public Transform ObjectEndPoint => _objectEndPoint;
        public Transform ObjectStartPoint => _objectStartPoint;
        public ParticleSystem FXDust => _fxDust;

        public void Init(TrashCanData trashCanData)
        {
            _mesh.GetComponent<MeshRenderer>().material.color = trashCanData.Color;
            TrashCanData = trashCanData;
            IsInteractable = true;
        }
        
        public void ToggleInteraction(bool enable) =>
            IsInteractable = enable;
    }
}