using UnityEngine;

namespace LevelData
{
    [CreateAssetMenu(fileName = nameof(LevelDifficultyData), menuName = "Level Data/Level Difficulty Data")]
    public class LevelDifficultyData : ScriptableObject
    {
        [SerializeField] private LevelType _levelType;
        [Space] 
        [SerializeField] private LevelObjectsData _levelObjectsData;
        [SerializeField] private float _countdownTime;

        public LevelType LevelType => _levelType;
        public LevelObjectsData ObjectsData => _levelObjectsData;
        public float CountdownTime => _countdownTime;
    }
}