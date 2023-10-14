using System.Collections.Generic;
using ObjectsData;
using UnityEngine;

namespace Main
{
    public class TrashObjectSpawner : MonoBehaviour
    {
        [SerializeField] private TrashData _trashData;
        [SerializeField] int _count = 10;
        [Space] 
        [SerializeField] private float _sideBound;
        [SerializeField] private float _topBound;
        [SerializeField] private float _bottomBound;

        private List<TrashObject> _trashObjects;

        private void Awake()
        {
            _trashObjects = new List<TrashObject>();
            Spawn(_count);
        }

        public void Spawn(int count)
        {
            for (int i = 0; i < count; i++)
            {
                TrashObject trashObject = SpawnObject();
                _trashObjects.Add(trashObject);
            }
        }

        private TrashObject SpawnObject() =>
            Instantiate(_trashData.Model, GetRandomPosition(), GetRandomRotation(), transform);

        private Vector3 GetRandomPosition() =>
            new (Random.Range(-_sideBound, _sideBound),
                Random.Range(1f, 2f),
                Random.Range(_bottomBound, _topBound));

        private Quaternion GetRandomRotation() =>
            Random.rotation;
    }
}