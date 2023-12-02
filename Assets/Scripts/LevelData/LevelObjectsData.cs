using System;
using UnityEngine;

namespace LevelData
{
    [Serializable]
    public class LevelObjectsData
    {
        [SerializeField] private int _trashCanCount;
        [SerializeField] private int _trashObjectForCanCount;
        [SerializeField] private int _trashObjectMaxCount;

        public int TrashCanCount => _trashCanCount;
        public int TrashObjectForCanCount => _trashObjectForCanCount;
        public int TrashObjectMaxCount => _trashObjectMaxCount;
    }
}