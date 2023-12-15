using UnityEngine;

namespace Core.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioData _audioData;
        
        public void Play() => 
            AudioManager.Instance.Play(_audioData);
        
        public void Stop() => 
            AudioManager.Instance.Stop(_audioData);
    }
}