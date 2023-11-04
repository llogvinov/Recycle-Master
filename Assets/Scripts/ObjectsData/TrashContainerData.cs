using UnityEngine;

namespace ObjectsData
{
    [CreateAssetMenu(fileName = nameof(TrashContainerData), menuName = "Object Data/Trash Container Data")]
    public class TrashContainerData : ScriptableObject
    {
        [SerializeField] private TrashType _type;
        [SerializeField] private Color _color;

        public TrashType Type => _type;

        public Color Color => _color;
    }
}