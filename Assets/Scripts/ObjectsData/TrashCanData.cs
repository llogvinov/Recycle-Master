using UnityEngine;

namespace ObjectsData
{
    [CreateAssetMenu(fileName = nameof(TrashCanData), menuName = "Object Data/Trash Can Data")]
    public class TrashCanData : ScriptableObject
    {
        [SerializeField] private TrashType _type;
        [SerializeField] private Color _color;

        public TrashType Type => _type;

        public Color Color => _color;
    }
}