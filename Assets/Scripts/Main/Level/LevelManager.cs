using System;
using System.Collections.Generic;
using System.Linq;
using LevelData;
using ObjectsData;
using UnityEngine;

namespace Main.Level
{
    public class LevelManager : MonoBehaviour
    {
        public Action LevelComplete;
        
        [SerializeField] private WallAdjuster _wallAdjuster;
        [SerializeField] private TrashCanSpawner _trashCanSpawnerPrefab;
        [SerializeField] private TrashObjectSpawner _trashObjectSpawnerPrefab;
        [Space]
        [SerializeField] private LevelDetailsData _levelDetailsData;
        [SerializeField] private bool _allowSimilarObjects;

        public TrashCanSpawner TrashCanSpawnerPrefab => _trashCanSpawnerPrefab;
        public TrashObjectSpawner TrashObjectSpawnerPrefab => _trashObjectSpawnerPrefab;
        public List<TrashObjectSpawner> TrashObjectSpawners => _trashObjectSpawners;
        public bool AllowSimilarObjects => _allowSimilarObjects;
        public LevelDifficultyData LevelDifficultyData { get; set; }

        private LevelBuilder _levelBuilder;
        private List<TrashObjectSpawner> _trashObjectSpawners;
        private LevelDetailsData currentLevelDetailsData;

        private void OnEnable() => 
            RecycleManager.AllObjectsOfSpawnerThrown += CheckAllSpawners;

        private void OnDisable() => 
            RecycleManager.AllObjectsOfSpawnerThrown -= CheckAllSpawners;

        private void CheckAllSpawners()
        {
            if (_trashObjectSpawners.All(spawner => spawner.AllObjectsThrown)) 
                LevelComplete?.Invoke();
        }

        public void BuildTutorialLevel(TrashCanData trashCanData)
        {
            _levelBuilder = new LevelBuilder(this);
            _wallAdjuster.AdjustAllWalls();
            _trashObjectSpawners = new List<TrashObjectSpawner>();

            _levelBuilder
                .ClearLevel()
                .SpawnTrashCans(trashCanData)
                .SpawnTrashObjects(trashCanData)
                .InvokeAllObjectSpawned();
        }

        public void BuildSpecificLevel()
        {
            _levelBuilder = new LevelBuilder(this);
            _wallAdjuster.AdjustAllWalls();
            _trashObjectSpawners = new List<TrashObjectSpawner>();

            _levelBuilder
                .ClearLevel()
                .SetLevelDetails(_levelDetailsData)
                .SetLevelDifficultyData(_levelDetailsData.Type)
                .SpawnTrashCans()
                .SpawnTrashObjects()
                .InvokeAllObjectSpawned();
        }

        public void BuildCurrentLevel()
        {
            _levelBuilder = new LevelBuilder(this);
            _wallAdjuster.AdjustAllWalls();
            _trashObjectSpawners = new List<TrashObjectSpawner>();

            currentLevelDetailsData = CachedLevel.CurrentLevelDetailsData;
            if (currentLevelDetailsData is null)
                throw new ArgumentNullException(nameof(currentLevelDetailsData), "cached level is null");
            
            _levelBuilder
                .ClearLevel()
                .SetLevelDetails(currentLevelDetailsData)
                .SetLevelDifficultyData(currentLevelDetailsData.Type)
                .SpawnTrashCans()
                .SpawnTrashObjects()
                .InvokeAllObjectSpawned();
        }

        public void BuildRandomLevel(LevelType levelType)
        {
            _levelBuilder = new LevelBuilder(this);
            _wallAdjuster.AdjustAllWalls();
            _trashObjectSpawners = new List<TrashObjectSpawner>();

            _levelBuilder
                .ClearLevel()
                .SetLevelDifficultyData(levelType)
                .GetRandomTrashCanDatas()
                .SpawnTrashCans()
                .GetRandomTrashDatas()
                .SpawnTrashObjects()
                .InvokeAllObjectSpawned();
        }

        public void ClearLevel() => 
            _levelBuilder.ClearLevel();
    }
}