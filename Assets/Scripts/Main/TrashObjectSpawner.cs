using System.Collections;
using System.Collections.Generic;
using ObjectsData;
using UnityEngine;

namespace Main
{
    public class TrashObjectSpawner : MonoBehaviour
    {
        private int _count;
        private TrashData _trashData;
        private List<TrashObject> _trashObjects;

        private const float SpawnOffset = 1f;
        
        public void Init(TrashData trashData, int count)
        {
            _trashObjects = new List<TrashObject>();
            _trashData = trashData;
            _count = count;
            Spawn(_count);
        }

        private void Spawn(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var trashObject = SpawnTrashObject();
                trashObject.Init(_trashData);
                _trashObjects.Add(trashObject);
            }
        }

        private TrashObject SpawnTrashObject() =>
            Instantiate(_trashData.Model, GetRandomPosition(), GetRandomRotation(), transform);

        private Vector3 GetRandomPosition() =>
            new (Random.Range(-WallAdjuster.HalfWidth + SpawnOffset, WallAdjuster.HalfWidth - SpawnOffset),
                Random.Range(1f, 2f),
                Random.Range(WallAdjuster.TempWallHeight + SpawnOffset, WallAdjuster.HalfHeight - SpawnOffset));

        private Quaternion GetRandomRotation() =>
            Random.rotation;
    }
}