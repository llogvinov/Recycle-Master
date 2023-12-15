using System;
using System.Collections.Generic;
using System.Linq;
using LevelData;
using ObjectsData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Level
{
    public class LevelBuilder
    {
        private Action AllObjectSpawned;
        
        public LevelBuilder InvokeAllObjectSpawned()
        {
            AllObjectSpawned?.Invoke();
            return this;
        }
        
        private LevelObjectsData _levelObjectsData => 
            _levelManager.LevelDifficultyData.ObjectsData;

        private List<TrashData> _trashDatas;
        private List<TrashCanData> _trashCanDatas;
        private readonly LevelManager _levelManager;

        public LevelBuilder(LevelManager levelManager) => 
            _levelManager = levelManager;

        public LevelBuilder SpawnTrashCans()
        {
            var trashCanSpawner = GameObject.Instantiate(_levelManager.TrashCanSpawnerPrefab);
            trashCanSpawner.Init(_trashCanDatas);
            
            return this;
        }

        public LevelBuilder GetRandomTrashCanDatas()
        {
            var trashCanDatas = new List<TrashCanData>();
            var trashCanDatasTemp = new List<TrashCanData>();
            trashCanDatasTemp.AddRange(ResourceLoader.TrashCanDatas);
                
            for (var i = 0; i < _levelObjectsData.TrashCanCount; i++)
            {
                var addingTrashCanIndex = Random.Range(0, trashCanDatasTemp.Count);
                var addingTrashCan = trashCanDatasTemp[addingTrashCanIndex];
                trashCanDatas.Add(addingTrashCan);
                trashCanDatasTemp.Remove(addingTrashCan);
            }

            _trashCanDatas = trashCanDatas;
            return this;
        }

        public LevelBuilder SpawnTrashObjects()
        {
            foreach (var trashData in _trashDatas)
            {
                var trashObjectSpawner = GameObject.Instantiate(_levelManager.TrashObjectSpawnerPrefab);
                trashObjectSpawner.Init(trashData, _levelObjectsData.TrashObjectMaxCount);
                _levelManager.TrashObjectSpawners.Add(trashObjectSpawner);
            }
            
            return this;
        }

        public LevelBuilder GetRandomTrashDatas()
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

            for (var i = 0; i < _levelObjectsData.TrashObjectForCanCount; i++)
            {
                var addingTrashObjectIndex = Random.Range(0, trashObjectDatasOfType.Count);
                var addingTrashObject = trashObjectDatasOfType[addingTrashObjectIndex];
                trashDatas.Add(addingTrashObject);
                if (!_levelManager.AllowSimilarObjects)
                    trashObjectDatasOfType.Remove(addingTrashObject);
            }

            return trashDatas;
        }

        public LevelBuilder SetLevelDetails(LevelDetailsData levelDetailsData)
        {
            _trashCanDatas = levelDetailsData.TrashCanDatas;
            _trashDatas = levelDetailsData.TrashDatas;
            
            return this;
        }

        public LevelBuilder SetLevelDifficultyData(LevelType levelType)
        {
            _levelManager.LevelDifficultyData = ResourceLoader.LevelDifficultyDatas
                .FirstOrDefault(l => l.LevelType == levelType);
            
            if (_levelManager.LevelDifficultyData == null) 
                Debug.LogError($"Level difficulty data of type {levelType} not found!");
            
            return this;
        }

        public LevelBuilder ClearLevel()
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