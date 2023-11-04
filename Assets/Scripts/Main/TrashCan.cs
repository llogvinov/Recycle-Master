using ObjectsData;
using UnityEngine;

namespace Main
{
    public class TrashCan : MonoBehaviour
    {
        [SerializeField] private ColliderChecker _checkCollider;
        [SerializeField] private Transform _objectEndPoint;

        public TrashContainerData TrashCanData { get; private set; }

        public Transform ObjectEndPoint => _objectEndPoint;

        public void Init(TrashContainerData trashCanData)
        {
            TrashCanData = trashCanData;
        }
        
        private void Awake()
        {
            _checkCollider.Init(this);
        }
    }
}