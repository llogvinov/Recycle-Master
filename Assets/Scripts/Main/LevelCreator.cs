using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private int _currentLevel;
        private LevelType _levelType;
        private Dictionary<TrashCanData, List<TrashData>> _levelDetails;

        public void GenerateLevel(LevelType levelType)
        {
            ClearLevel();
            
            _wallAdjuster.AdjustAllWalls();
            
            var levelDifficultyData = GetLevelDifficultyData(levelType);

            if (levelDifficultyData == null)
            {
                Debug.LogError($"Level difficulty data of type {levelType} not found!");
            }

            _levelType = levelType;
            _levelDetails = new Dictionary<TrashCanData, List<TrashData>>();
            var trashCanDatas = SpawnTrashCans(levelDifficultyData.ObjectsData.TrashCanCount);

            foreach (var trashCanData in trashCanDatas)
            {
                var trashDatas = SpawnTrashObject(trashCanData, 
                    levelDifficultyData.ObjectsData.TrashObjectForCanCount, 
                    levelDifficultyData.ObjectsData.TrashObjectMaxCount);
                _levelDetails.TryAdd(trashCanData, trashDatas);
            }

            AllObjectSpawned?.Invoke();

            Timer.Instance.StartCountdown(levelDifficultyData.CountdownTime);
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

        private List<TrashData> SpawnTrashObject(TrashCanData trashCanData, int trashObjectForCanCount, int trashObjectMaxCount)
        {
            var trashObjectDatasOfType = 
                ResourceLoader.TrashDatas.Where(data => data.Type == trashCanData.Type).ToList();
            var trashDatas = new List<TrashData>();

            for (var i = 0; i < trashObjectForCanCount; i++)
            {
                var addingTrashObjectIndex = Random.Range(0, trashObjectDatasOfType.Count);
                var addingTrashObject = trashObjectDatasOfType[addingTrashObjectIndex];
                var trashObjectSpawner = Instantiate(_trashObjectSpawner);
                trashObjectSpawner.Init(addingTrashObject, trashObjectMaxCount);
                trashDatas.Add(addingTrashObject);
                if (!_allowSimilarObjects) 
                    trashObjectDatasOfType.Remove(addingTrashObject);
            }

            return trashDatas;
        }

        public void ClearLevel()
        {
            Timer.Instance.StopCountdown();
            
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

        private void LogDetails()
        {
            var s = new StringBuilder();
            foreach (var detail in _levelDetails)
            {
                var d = "";
                foreach (var trashData in detail.Value)
                    d += $"{trashData.Title}, ";
                s.Append($"{detail.Key.Type} - {d}");
            }

            Debug.Log(s.ToString());
        }
    }
}