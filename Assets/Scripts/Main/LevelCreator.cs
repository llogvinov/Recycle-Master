using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LevelData;
using ObjectsData;
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
        [Space]
        [SerializeField] private LevelDetailsData _levelDetailsData;
        [SerializeField] private bool _allowSimilarObjects;

        private int _currentLevel;
        private Dictionary<TrashCanData, List<TrashData>> _levelDetails;
        
        public void GenerateLevel()
        {
            if (_levelDetailsData is null)
                Debug.LogError($"Set the {nameof(_levelDetailsData)} to generate level");
                
            GenerateLevel(_levelDetailsData);
        }
        
        public void GenerateLevel(LevelType levelType)
        {
            ClearLevel();
            _wallAdjuster.AdjustAllWalls();
            
            var levelDifficultyData = GetLevelDifficultyData(levelType);
            if (levelDifficultyData == null)
            {
                Debug.LogError($"Level difficulty data of type {levelType} not found!");
            }

            _levelDetails = new Dictionary<TrashCanData, List<TrashData>>();
            var trashCanDatas = SpawnTrashCans(levelDifficultyData.ObjectsData.TrashCanCount);
            foreach (var trashCanData in trashCanDatas)
            {
                var trashDatas = SpawnTrashObject(trashCanData, levelDifficultyData);
                _levelDetails.TryAdd(trashCanData, trashDatas);
            }

            AllObjectSpawned?.Invoke();
            
            if (Timer.HasInstance)
                Timer.Instance.StartCountdown(levelDifficultyData.CountdownTime);
        }

        private void GenerateLevel(LevelDetailsData levelDetailsData)
        {
            ClearLevel();
            _wallAdjuster.AdjustAllWalls();
            _levelDetails = new Dictionary<TrashCanData, List<TrashData>>();
            var levelDifficultyData = GetLevelDifficultyData(levelDetailsData.Type);
            SpawnTrashCans(levelDetailsData.TrashCanDatas);
            SpawnTrashObjects(levelDetailsData.TrashDatas, levelDifficultyData.ObjectsData.TrashObjectMaxCount);
            
            AllObjectSpawned?.Invoke();
           
            if (Timer.HasInstance)
                Timer.Instance.StartCountdown(levelDifficultyData.CountdownTime);
        }

        private LevelDifficultyData GetLevelDifficultyData(LevelType levelType) => 
            ResourceLoader.LevelDifficultyDatas.FirstOrDefault(l => l.LevelType == levelType);

        private void SpawnTrashCans(List<TrashCanData> trashCanDatas)
        {
            var trashCanSpawner = Instantiate(_trashCanSpawner);
            trashCanSpawner.Init(trashCanDatas);
        }

        private List<TrashCanData> SpawnTrashCans(int trashCanCount)
        {
            var trashCanDatas = GetTrashCanDatas();
            SpawnTrashCans(trashCanDatas);
            return trashCanDatas;

            List<TrashCanData> GetTrashCanDatas()
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

                return trashCanDatas;
            }
        }

        private void SpawnTrashObjects(List<TrashData> trashDatas, int trashObjectMaxCount)
        {
            foreach (var trashData in trashDatas)
            {
                var trashObjectSpawner = Instantiate(_trashObjectSpawner);
                trashObjectSpawner.Init(trashData, trashObjectMaxCount);
            }
        }

        private List<TrashData> SpawnTrashObject(TrashCanData trashCanData, LevelDifficultyData levelDifficultyData)
        {
            var trashDatas = GetTrashDatas();
            SpawnTrashObjects(trashDatas, levelDifficultyData.ObjectsData.TrashObjectMaxCount);
            return trashDatas;

            List<TrashData> GetTrashDatas()
            {
                var trashObjectDatasOfType =
                    ResourceLoader.TrashDatas.Where(data => data.Type == trashCanData.Type).ToList();
                var trashDatas = new List<TrashData>();

                for (var i = 0; i < levelDifficultyData.ObjectsData.TrashObjectForCanCount; i++)
                {
                    var addingTrashObjectIndex = Random.Range(0, trashObjectDatasOfType.Count);
                    var addingTrashObject = trashObjectDatasOfType[addingTrashObjectIndex];
                    trashDatas.Add(addingTrashObject);
                    if (!_allowSimilarObjects)
                        trashObjectDatasOfType.Remove(addingTrashObject);
                }

                return trashDatas;
            }
        }

        public void ClearLevel()
        {
            if (Timer.HasInstance)
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

#if UNITY_EDITOR
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
#endif
    }
}