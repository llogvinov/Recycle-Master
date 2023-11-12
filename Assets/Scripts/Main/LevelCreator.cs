using System;
using System.Collections.Generic;
using ObjectsData;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class LevelCreator : MonoBehaviour
    {
        public static Action AllObjectSpawned; 
        
        [SerializeField] private TrashCanSpawner _trashCanSpawner;
        [SerializeField] private TrashObjectSpawner _trashObjectSpawner;
        [Space]
        [SerializeField] private Button _easyLevel;
        [SerializeField] private Button _mediumLevel;
        [SerializeField] private Button _hardLevel;
        [SerializeField] private Button _superHardLevel;

        private TrashData[] _trashDatas;
        private TrashCanData[] _trashCanDatas;

        private void Start()
        {
            _easyLevel.onClick.AddListener(GenerateEasyLevel);
            _mediumLevel.onClick.AddListener(GenerateMediumLevel);
            _hardLevel.onClick.AddListener(GenerateHardLevel);
            _superHardLevel.onClick.AddListener(GenerateSuperHardLevel);

            _trashDatas = Resources.LoadAll<TrashData>("ObjectsData/Trash");
            _trashCanDatas = Resources.LoadAll<TrashCanData>("ObjectsData/TrashCan");
        }

        private void GenerateEasyLevel() => GenerateLevel(2, 2, 10);
        private void GenerateMediumLevel() => GenerateLevel(3, 3, 10);
        private void GenerateHardLevel() => GenerateLevel(4, 4, 10);
        private void GenerateSuperHardLevel() => GenerateLevel(4, 5, 15);

        private void GenerateLevel(int trashCanCount, int trashObjectForCanCount, int trashObjectMaxCount)
        {
            var trashCanDatas = new List<TrashCanData>();
            
            for (int i = 0; i < trashCanCount; i++)
            {
                trashCanDatas.Add(_trashCanDatas[i]);
            }
            
            _trashCanSpawner.Init(trashCanDatas);

            for (int i = 0; i < trashObjectForCanCount; i++)
            {
                var trashObjectSpawner = Instantiate(_trashObjectSpawner);
                trashObjectSpawner.Init(_trashDatas[i], trashObjectMaxCount);
            }
            AllObjectSpawned?.Invoke();
        }
    }
}