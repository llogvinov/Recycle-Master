using System;
using System.Collections.Generic;
using System.Linq;
using LevelData;
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
        [SerializeField] private Button _clearButton;

        private TrashData[] _trashDatas;
        private TrashCanData[] _trashCanDatas;
        private LevelDifficultyData[] _levelDifficultyDatas;

        private void Start()
        {
            _easyLevel.onClick.AddListener(GenerateEasyLevel);
            _mediumLevel.onClick.AddListener(GenerateMediumLevel);
            _hardLevel.onClick.AddListener(GenerateHardLevel);
            _superHardLevel.onClick.AddListener(GenerateSuperHardLevel);
            _clearButton.onClick.AddListener(ClearLevel);

            _trashDatas = Resources.LoadAll<TrashData>("ObjectsData/Trash");
            _trashCanDatas = Resources.LoadAll<TrashCanData>("ObjectsData/TrashCan");
            _levelDifficultyDatas = Resources.LoadAll<LevelDifficultyData>("LevelData/LevelDifficulty");
        }

        private void OnDestroy()
        {
            _easyLevel.onClick.RemoveListener(GenerateEasyLevel);
            _mediumLevel.onClick.RemoveListener(GenerateMediumLevel);
            _hardLevel.onClick.RemoveListener(GenerateHardLevel);
            _superHardLevel.onClick.RemoveListener(GenerateSuperHardLevel);
            _clearButton.onClick.RemoveListener(ClearLevel);
        }

        private void ClearLevel()
        {
            var canSpawner = FindObjectOfType<TrashCanSpawner>();
            if (canSpawner != null)
                Destroy(canSpawner.gameObject);
            
            var objSpawners = FindObjectsOfType<TrashObjectSpawner>();
            if (objSpawners!= null && objSpawners.Length > 0)
            {
                foreach (var objSpawner in objSpawners) 
                    Destroy(objSpawner.gameObject);
            }
        }

        private void GenerateEasyLevel() => GenerateLevel(LevelType.Easy);
        private void GenerateMediumLevel() => GenerateLevel(LevelType.Medium);
        private void GenerateHardLevel() => GenerateLevel(LevelType.Hard);
        private void GenerateSuperHardLevel() => GenerateLevel(LevelType.SuperHard);
        
        private void GenerateLevel(LevelType levelType)
        {
            ClearLevel();
            
            var levelDifficultyData = GetLevelDifficultyData(levelType);

            if (levelDifficultyData == null)
            {
                Debug.LogError($"Level difficulty data of type {levelType} not found!");
            }

            var trashCanDatas = SpawnTrashCans(levelDifficultyData.ObjectsData.TrashCanCount);

            foreach (var trashCanData in trashCanDatas)
            {
                SpawnTrashObject(trashCanData, 
                    levelDifficultyData.ObjectsData.TrashObjectForCanCount, 
                    levelDifficultyData.ObjectsData.TrashObjectMaxCount);
            }
            
            AllObjectSpawned?.Invoke();
        }
        
        private LevelDifficultyData GetLevelDifficultyData(LevelType levelType) => 
            _levelDifficultyDatas.FirstOrDefault(l => l.LevelType == levelType);

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

            var trashCanSpawner = Instantiate(_trashCanSpawner);
            trashCanSpawner.Init(trashCanDatas);
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