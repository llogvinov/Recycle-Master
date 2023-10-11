using UnityEngine;

namespace ObjectsData
{
    [CreateAssetMenu(fileName = nameof(TrashContainerData), menuName = "Object Data/Trash Container Data")]
    public class TrashContainerData : ScriptableObject
    {
        [SerializeField] private TrashType _type;

        public TrashType Type => _type;
    }
}