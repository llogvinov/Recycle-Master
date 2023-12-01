using System.Collections.Generic;
using LevelData;
using ObjectsData;

namespace Core.Data
{
    public class LevelProgressData
    {
        public int CurrentLevel { get; private set; }
        public LevelType LevelType { get; private set; }
        public Dictionary<TrashCanData, List<TrashData>> LevelDetails { get; private set; }

        public LevelProgressData(int currentLevel, LevelType levelType, Dictionary<TrashCanData, List<TrashData>> levelDetails)
        {
            CurrentLevel = currentLevel;
            LevelType = levelType;
            LevelDetails = levelDetails;
        }
    }
}