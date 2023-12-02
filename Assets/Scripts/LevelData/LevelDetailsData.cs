using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Main;
using ObjectsData;
using UnityEngine;

namespace LevelData
{
    [CreateAssetMenu(fileName = nameof(LevelDetailsData), menuName = "Level Data/Level Details Data")]
    public class LevelDetailsData : ScriptableObject
    {
        [SerializeField] private uint _levelNumber;
        [SerializeField] private LevelType _levelType;
        [SerializeField] private List<TrashCanData> _trashCanDatas;
        [SerializeField] private List<TrashData> _trashDatas;
        [Space]
        [SerializeField] private bool _allowSimilarObjects;

        public uint LevelNumber => _levelNumber;
        public LevelType Type => _levelType;
        public List<TrashCanData> TrashCanDatas => _trashCanDatas;
        public List<TrashData> TrashDatas => _trashDatas;

        public void SetEasyLevelRandomData() => GenerateLevel(LevelType.Easy);
        public void SetMediumLevelRandomData() => GenerateLevel(LevelType.Medium);
        public void SetHardLevelRandomData() => GenerateLevel(LevelType.Hard);
        public void SetSuperHardRandomData() => GenerateLevel(LevelType.SuperHard);

        private void GenerateLevel(LevelType levelType)
        {
            _levelNumber = ExtractLevelValue(name);
            
            var levelDifficultyData = GetLevelDifficultyData(levelType);

            if (levelDifficultyData == null) 
                Debug.LogError($"Level difficulty data of type {levelType} not found!");

            _levelType = levelType;
            
            _trashCanDatas.Clear();
            _trashCanDatas = GetRandomTrashCanDatas(levelDifficultyData.ObjectsData.TrashCanCount);

            _trashDatas.Clear();
            foreach (var trashCanData in _trashCanDatas)
            {
                var trashDatas = GetRandomTrashDatas(trashCanData, 
                    levelDifficultyData.ObjectsData.TrashObjectForCanCount, 
                    levelDifficultyData.ObjectsData.TrashObjectMaxCount);
                _trashDatas.AddRange(trashDatas);
            }
        }
        
        private uint ExtractLevelValue(string input)
        {
            var pattern = @"(\d+)";
            var match = Regex.Match(input, pattern);
            if (match.Success)
                if (uint.TryParse(match.Value, out uint result))
                    return result;
            return 0;
        }

        private LevelDifficultyData GetLevelDifficultyData(LevelType levelType) => 
            ResourceLoader.LevelDifficultyDatas.FirstOrDefault(l => l.LevelType == levelType);

        private List<TrashCanData> GetRandomTrashCanDatas(int trashCanCount)
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

        private List<TrashData> GetRandomTrashDatas(TrashCanData trashCanData, int trashObjectForCanCount, int trashObjectMaxCount)
        {
            var trashObjectDatasOfType = 
                ResourceLoader.TrashDatas.Where(data => data.Type == trashCanData.Type).ToList();
            var trashDatas = new List<TrashData>();

            for (var i = 0; i < trashObjectForCanCount; i++)
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
}