using UnityEngine;

namespace Core.Audio
{
    [CreateAssetMenu(fileName = nameof(AudioData), menuName = "Audio Data")]
    public class AudioData : ScriptableObject
    {
        [SerializeField] private AudioType _type;
        [SerializeField] private AudioClip _clip;
        [Range(0f,1f)]
        [SerializeField] private float _volume = 1f;

        public AudioType Type => _type;

        public AudioClip Clip => _clip;

        public float Volume => _volume;
    }
}