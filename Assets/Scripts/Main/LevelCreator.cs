using System;
using System.Collections.Generic;
using System.Linq;
using LevelData;
using ObjectsData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main
{
    public class LevelCreator
    {
        private Action AllObjectSpawned;
        
        public LevelCreator InvokeAllObjectSpawned()
        {
            AllObjectSpawned?.Invoke();
            return this;
        }

        public LevelDifficultyData LevelDifficultyData;
        
        private List<TrashData> _trashDatas;
        private List<TrashCanData> _trashCanDatas;
        private readonly LevelManager _levelManager;

        public LevelCreator(LevelManager levelManager) => 
            _levelManager = levelManager;

        public LevelCreator SpawnTrashCans()
        {
            var trashCanSpawner = GameObject.Instantiate(_levelManager.TrashCanSpawnerPrefab);
            trashCanSpawner.Init(_trashCanDatas);
            
            return this;
        }

        public LevelCreator SpawnTrashObjects()
        {
            foreach (var trashData in _trashDatas)
            {
                var trashObjectSpawner = GameObject.Instantiate(_levelManager.TrashObjectSpawnerPrefab);
                trashObjectSpawner.Init(trashData, LevelDifficultyData.ObjectsData.TrashObjectMaxCount);
                _levelManager.TrashObjectSpawners.Add(trashObjectSpawner);
            }
            
            return this;
        }

        public LevelCreator GetRandomTrashCanDatas()
        {
            var trashCanDatas = new List<TrashCanData>();
            var trashCanDatasTemp = new List<TrashCanData>();
            trashCanDatasTemp.AddRange(ResourceLoader.TrashCanDatas);
                
            for (var i = 0; i < LevelDifficultyData.ObjectsData.TrashCanCount; i++)
            {
                var addingTrashCanIndex = Random.Range(0, trashCanDatasTemp.Count);
                var addingTrashCan = trashCanDatasTemp[addingTrashCanIndex];
                trashCanDatas.Add(addingTrashCan);
                trashCanDatasTemp.Remove(addingTrashCan);
            }

            _trashCanDatas = trashCanDatas;
            return this;
        }

        public LevelCreator GetRandomTrashDatas()
        {
            _trashDatas = new List<TrashData>();
            foreach (var trashCanData in _trashCanDatas) 
                _trashDatas.AddRange(GetRandomTrashDatas(trashCanData));

            return this;
        }

        private List<TrashData> GetRandomTrashDatas(TrashCanData trashCanData)
        {
            var trashObjectDatasOfType =
                ResourceLoader.TrashDatas.Where(data => data.Type == trashCanData.Type).ToList();
            var trashDatas = new List<TrashData>();

            for (var i = 0; i < LevelDifficultyData.ObjectsData.TrashObjectForCanCount; i++)
            {
                var addingTrashObjectIndex = Random.Range(0, trashObjectDatasOfType.Count);
                var addingTrashObject = trashObjectDatasOfType[addingTrashObjectIndex];
                trashDatas.Add(addingTrashObject);
                if (!_levelManager.AllowSimilarObjects)
                    trashObjectDatasOfType.Remove(addingTrashObject);
            }

            return trashDatas;
        }

        public LevelCreator SetLevelDetails(LevelDetailsData levelDetailsData)
        {
            _trashCanDatas = levelDetailsData.TrashCanDatas;
            _trashDatas = levelDetailsData.TrashDatas;
            
            return this;
        }

        public LevelCreator SetLevelDifficultyData(LevelType levelType)
        {
            LevelDifficultyData = ResourceLoader.LevelDifficultyDatas
                .FirstOrDefault(l => l.LevelType == levelType);
            
            if (LevelDifficultyData == null) 
                Debug.LogError($"Level difficulty data of type {levelType} not found!");
            
            return this;
        }

        public LevelCreator ClearLevel()
        {
            if (Timer.HasInstance)
                Timer.Instance.StopCountdown();
            
            _trashDatas?.Clear();
            _trashCanDatas?.Clear();
            
            var canSpawner = GameObject.FindObjectOfType<TrashCanSpawner>();
            if (canSpawner != null)
                GameObject.Destroy(canSpawner.gameObject);
            
            var objSpawners = GameObject.FindObjectsOfType<TrashObjectSpawner>();
            if (objSpawners!= null && objSpawners.Length > 0)
            {
                foreach (var objSpawner in objSpawners) 
                    GameObject.Destroy(objSpawner.gameObject);
            }

            return this;
        }
    }
}