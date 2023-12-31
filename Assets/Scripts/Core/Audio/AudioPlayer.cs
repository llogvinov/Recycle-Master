using UnityEngine;

namespace Core.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioData _audioData;

        public void Switch(bool play)
        {
            if (play) 
                AudioManager.Instance.Play(_audioData);
            else 
                AudioManager.Instance.Stop(_audioData);
        }
    }
}