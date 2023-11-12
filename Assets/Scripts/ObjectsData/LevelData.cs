using UnityEngine;

namespace ObjectsData
{
    [CreateAssetMenu(fileName = nameof(LevelData), menuName = "Object Data/Level Data")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private LevelType _levelType;

        public void GetData()
        {
            int trashCanCount;
            int trashObjectForTrashCanCount;
            int eachTrashObjectCount;
        }
    }

    public enum LevelType
    {
        Undefined,
        Easy,
        Medium,
        Hard,
        SuperHard,
    }
}