using System.Collections.Generic;
using ObjectsData;
using UnityEngine;

namespace Main
{
    public class TrashCanSpawner : MonoBehaviour
    {
        [SerializeField] private TrashCan _trashCanPrefab;
        [SerializeField] private List<TrashCanData> _trashCanDatas;

        private List<TrashCan> _trashCans;

        private void Awake()
        {
            _trashCans = new List<TrashCan>();
            Spawn();
        }

        private void Spawn()
        {
            foreach (var trashCanData in _trashCanDatas)
            {
                var trashCan = SpawnTrashCan();
                trashCan.Init(trashCanData);
                _trashCans.Add(trashCan);
            }
        }

        private TrashCan SpawnTrashCan() =>
            Instantiate(_trashCanPrefab, GetCanPosition(), Quaternion.identity, transform);

        private Vector3 GetCanPosition() =>
            new(0f, 0f, -4f);
    }
}