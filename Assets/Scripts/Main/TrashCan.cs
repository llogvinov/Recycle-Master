using ObjectsData;
using UnityEngine;

namespace Main
{
    public class TrashCan : MonoBehaviour
    {
        [SerializeField] private GameObject _mesh;
        [SerializeField] private ColliderChecker _checkCollider;
        [SerializeField] private Transform _objectEndPoint;

        public TrashCanData TrashCanData { get; private set; }

        public Transform ObjectEndPoint => _objectEndPoint;

        public void Init(TrashCanData trashCanData)
        {
            _mesh.GetComponent<MeshRenderer>().material.color = trashCanData.Color;
            TrashCanData = trashCanData;
        }
        
        private void Awake()
        {
            _checkCollider.Init(this);
        }
    }
}