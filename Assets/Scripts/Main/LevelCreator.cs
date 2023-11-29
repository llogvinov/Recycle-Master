using System;
using System.Collections.Generic;
using System.Linq;
using LevelData;
using ObjectsData;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main
{
    public class LevelCreator : MonoBehaviour
    {
        public static Action AllObjectSpawned;

        [SerializeField] private WallAdjuster _wallAdjuster;
        [SerializeField] private TrashCanSpawner _trashCanSpawner;
        [SerializeField] private TrashObjectSpawner _trashObjectSpawner;
        [SerializeField] private bool _allowSimilarObjects;
        
        public void GenerateLevel(LevelType levelType)
        {
            ClearLevel();
            
            _wallAdjuster.AdjustAllWalls();
            
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

            var timer = FindObjectOfType<UITimer>();
            timer.StartCountdown(levelDifficultyData.CountdownTime);
        }
        
        private LevelDifficultyData GetLevelDifficultyData(LevelType levelType) => 
            ResourceLoader.LevelDifficultyDatas.FirstOrDefault(l => l.LevelType == levelType);

        private List<TrashCanData> SpawnTrashCans(int trashCanCount)
        {
            var trashCanDatas = new List<TrashCanData>();
            var trashCanDatasTemp = new List<TrashCanData>();
            trashCanDatasTemp.AddRange(ResourceLoader.TrashCanDatas);

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
            var trashObjectDatasOfType = ResourceLoader.TrashDatas.Where(data => data.Type == trashCanData.Type).ToList();

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
        
        public void ClearLevel()
        {
            var timer = FindObjectOfType<UITimer>();
            timer.StopCountdown();
            
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
    }
}