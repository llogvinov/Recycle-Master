using System.Collections;
using System.Collections.Generic;
using ObjectsData;
using UnityEngine;

namespace Main
{
    public class TrashObjectSpawner : MonoBehaviour
    {
        [SerializeField] private TrashData _trashData;
        [SerializeField] int _count;

        private List<TrashObject> _trashObjects;

        private const float SpawnOffset = 1f;

        private void Awake()
        {
            _trashObjects = new List<TrashObject>();
            StartCoroutine(DelayedSpawn());
        }

        private IEnumerator DelayedSpawn()
        {
            yield return new WaitForSeconds(0.5f);
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