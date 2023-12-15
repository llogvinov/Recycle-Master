using System.Collections.Generic;
using System.Linq;
using Core;
using LevelData;
using UnityEngine;

namespace Main.Level
{
    public class LevelManager : MonoBehaviour
    {
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
        public Game Game { get; set; }

        private LevelBuilder _levelBuilder;
        private List<TrashObjectSpawner> _trashObjectSpawners;

        private void OnEnable() => 
            RecycleManager.AllObjectsOfSpawnerThrown += CheckAllSpawners;

        private void OnDisable() => 
            RecycleManager.AllObjectsOfSpawnerThrown -= CheckAllSpawners;

        private void CheckAllSpawners()
        {
            if (Game is null) return;

            if (_trashObjectSpawners.All(spawner => spawner.AllObjectsThrown)) 
                Game.GameOver(GameOverCondition.Won);
        }

        public void GenerateSpecificLevel()
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
           
            if (Timer.HasInstance)
                Timer.Instance.StartCountdown(_levelBuilder.LevelDifficultyData.CountdownTime);
        }

        public void GenerateRandomLevel(LevelType levelType)
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

            if (Timer.HasInstance)
                Timer.Instance.StartCountdown(_levelBuilder.LevelDifficultyData.CountdownTime);
        }

        public void ClearLevel() => 
            _levelBuilder.ClearLevel();
    }
}