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
            var xPositions = GetCanXPositions(_trashCanDatas.Count);
            for (int i = 0; i < _trashCanDatas.Count; i++)
            {
                var trashCan = SpawnTrashCan(xPositions[i]);
                trashCan.Init(_trashCanDatas[i]);
                _trashCans.Add(trashCan);
            }
        }
        
        private TrashCan SpawnTrashCan(float xPosition) =>
            Instantiate(_trashCanPrefab, GetCanPosition(xPosition), Quaternion.identity, transform);

        private Vector3 GetCanPosition(float xPosition) => 
            new(xPosition, 0f, -4f);

        private float[] GetCanXPositions(int count)
        {
            switch (count)
            {
                case 1:
                    return new[] {0f};
                case 2:
                    return new[] {-1f, 1f};
                case 3:
                    return new[] {-2f, 0f, 2f};
                case 4:
                    return new[] {-3f, -1f, 1f, 3f};
                default:
                    return null;
            }
        }
    }
}