using System;
using System.Collections.Generic;
using System.Linq;
using LevelData;
using Main;
using ObjectsData;
using UI.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.Presenters
{
    public class LevelCreatorPresenter : MonoBehaviour
    {
        public static Action AllObjectSpawned; 
        
        [SerializeField] private LevelCreatorView _view;
        [SerializeField] private TrashCanSpawner _trashCanSpawner;
        [SerializeField] private TrashObjectSpawner _trashObjectSpawner;
        [SerializeField] private bool _allowSimilarObjects;

        private TrashData[] _trashDatas;
        private TrashCanData[] _trashCanDatas;
        private LevelDifficultyData[] _levelDifficultyDatas;

        public LevelCreatorView View => _view;
        
        private void Start()
        {
            _view.EasyLevel.onClick.AddListener(GenerateEasyLevel);
            _view.MediumLevel.onClick.AddListener(GenerateMediumLevel);
            _view.HardLevel.onClick.AddListener(GenerateHardLevel);
            _view.SuperHardLevel.onClick.AddListener(GenerateSuperHardLevel);
            _view.ClearButton.onClick.AddListener(ClearLevel);

            _trashDatas = Resources.LoadAll<TrashData>("ObjectsData/Trash");
            _trashCanDatas = Resources.LoadAll<TrashCanData>("ObjectsData/TrashCan");
            _levelDifficultyDatas = Resources.LoadAll<LevelDifficultyData>("LevelData/LevelDifficulty");
        }

        private void OnDestroy()
        {
            _view.EasyLevel.onClick.RemoveListener(GenerateEasyLevel);
            _view.MediumLevel.onClick.RemoveListener(GenerateMediumLevel);
            _view.HardLevel.onClick.RemoveListener(GenerateHardLevel);
            _view.SuperHardLevel.onClick.RemoveListener(GenerateSuperHardLevel);
            _view.ClearButton.onClick.RemoveListener(ClearLevel);
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