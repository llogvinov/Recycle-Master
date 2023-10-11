using Main;
using UnityEngine;

namespace ObjectsData
{
    [CreateAssetMenu(fileName = nameof(TrashData), menuName = "Object Data/Trash Data")]
    public class TrashData : ScriptableObject
    {
        [SerializeField] private TrashType _type;
        [SerializeField] private string _title;
        [SerializeField] private TrashObject _model;

        public TrashType Type => _type;

        public string Title => _title;

        public TrashObject Model => _model;
    }
}