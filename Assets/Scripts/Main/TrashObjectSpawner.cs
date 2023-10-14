using System.Collections.Generic;
using ObjectsData;
using UnityEngine;

namespace Main
{
    public class TrashObjectSpawner : MonoBehaviour
    {
        [SerializeField] private TrashData _trashData;

        private List<TrashObject> _trashObjects;

        private void Awake()
        {
            _trashObjects = new List<TrashObject>();
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
            new Vector3(0, 0, 0);

        private Quaternion GetRandomRotation() =>
            Random.rotation;
    }
}