using System;
using System.Collections.Generic;
using System.Linq;
using ObjectsData;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Main
{
    public class LevelCreator : MonoBehaviour
    {
        public static Action AllObjectSpawned; 
        
        [SerializeField] private TrashCanSpawner _trashCanSpawner;
        [SerializeField] private TrashObjectSpawner _trashObjectSpawner;
        [SerializeField] private bool _allowSimilarObjects;
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
            var trashCanDatas = SpawnTrashCans(trashCanCount);

            foreach (var trashCanData in trashCanDatas)
                SpawnTrashObject(trashCanData, trashObjectForCanCount, trashObjectMaxCount);
            
            AllObjectSpawned?.Invoke();
        }

        private List<TrashCanData> SpawnTrashCans(int trashCanCount)
        {
            var trashCanDatas = new List<TrashCanData>();
            var trashCanDatasTemp = new List<TrashCanData>();
            trashCanDatasTemp.AddRange(_trashCanDatas);

            for (var i = 0; i < trashCanCount; i++)
            {
                var addingTrashCanIndex = Random.Range(0, trashCanDatasTemp.Count);
                var addingTrashCan = trashCanDatasTemp[addingTrashCanIndex];
                trashCanDatas.Add(addingTrashCan);
                trashCanDatasTemp.Remove(addingTrashCan);
            }

            _trashCanSpawner.Init(trashCanDatas);
            return trashCanDatas;
        }

        private void SpawnTrashObject(TrashCanData trashCanData, int trashObjectForCanCount, int trashObjectMaxCount)
        {
            var trashObjectDatasOfType = _trashDatas.Where(data => data.Type == trashCanData.Type).ToList();

            for (var i = 0; i < trashObjectForCanCount; i++)
            {
                var addingTrashObjectIndex = Random.Range(0, trashObjectDatasOfType.Count);
                var addingTrashObject = trashObjectDatasOfType[addingTrashObjectIndex];
                var trashObjectSpawner = Instantiate(_trashObjectSpawner);
                trashObjectSpawner.Init(addingTrashObject, trashObjectMaxCount);
                if (!_allowSimilarObjects) 
                    trashObjectDatasOfType.Remove(addingTrashObject);
            }
        }
    }
}