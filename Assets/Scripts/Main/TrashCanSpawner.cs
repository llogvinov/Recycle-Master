using System;
using System.Collections.Generic;
using ObjectsData;
using UnityEngine;

namespace Main
{
    public class TrashCanSpawner : MonoBehaviour
    {
        [SerializeField] private TrashCan _trashCanPrefab;
        
        private List<TrashCanData> _trashCanDatas;
        private List<TrashCan> _trashCans;

        public void Init(List<TrashCanData> trashCanDatas)
        {
            _trashCans = new List<TrashCan>();
            _trashCanDatas = trashCanDatas;
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
            new(xPosition, 0f, SpawnBounds.TempBound - (SpawnBounds.TempBound - SpawnBounds.BottomBound) / 2);

        private float[] GetCanXPositions(int count)
        {
            switch (count)
            {
                case 1:
                    return new[] {0f};
                case 2:
                    return new[] {-1f, 1f};
                case 3:
                    return new[] {-1.8f, 0f, 1.8f};
                case 4:
                    return new[] {-2.4f, -0.8f, 0.8f, 2.4f};
                default:
                    return null;
            }
        }
    }
}