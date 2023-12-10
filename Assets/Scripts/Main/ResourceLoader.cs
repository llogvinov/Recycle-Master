using LevelData;
using ObjectsData;
using UnityEngine;

namespace Main
{
    public static class ResourceLoader
    {
        private static TrashData[] _trashDatas;
        public static TrashData[] TrashDatas => 
            _trashDatas ?? Resources.LoadAll<TrashData>("ObjectsData/Trash");
        
        private static TrashCanData[] _trashCanDatas;
        public static TrashCanData[] TrashCanDatas => 
            _trashCanDatas ?? Resources.LoadAll<TrashCanData>("ObjectsData/TrashCan");
        
        private static LevelDifficultyData[] _levelDifficultyDatas;
        public static LevelDifficultyData[] LevelDifficultyDatas => 
            _levelDifficultyDatas ?? Resources.LoadAll<LevelDifficultyData>("LevelData/LevelDifficulty");
    }
}