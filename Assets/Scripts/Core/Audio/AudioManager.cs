using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace Core.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundsSource;

        private const float SwitchTransitionTime = 0.5f;

        #region Singleton

        private static AudioManager _instance = null;
        
        public static AudioManager Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = FindObjectOfType<AudioManager>();
                    if (_instance is null)
                    {
                        var singletonObject = new GameObject("AudioManager");
                        _instance = singletonObject.AddComponent<AudioManager>();
                    }
                }

                return _instance;
            }
        }

        public static bool HasInstance => _instance is not null;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    
        #endregion

        public void Play(AudioData audioData)
        {
            switch (audioData.Type)
            {
                case AudioType.Music:
                    PlayMusic(audioData);
                    break;
                case AudioType.Sound:
                    PlaySound(audioData);
                    break;
                default:
                    throw new InvalidEnumArgumentException("Wrong Audio Type");
            }
        }
    
        private void PlayMusic(AudioData audioData)
        {
            _musicSource.clip = audioData.Clip;
            _musicSource.volume = audioData.Volume;
            StartCoroutine(SmoothSwitchMusic(audioData, true));
        }

        private void PlaySound(AudioData audioData) => 
            _soundsSource.PlayOneShot(audioData.Clip, audioData.Volume);

        public void Stop(AudioData audioData)
        {
            switch (audioData.Type)
            {
                case AudioType.Music:
                    StopMusic(audioData);
                    break;
                case AudioType.Sound:
                    StopSound(audioData);
                    break;
                default:
                    throw new InvalidEnumArgumentException("Wrong Audio Type");
            }
        }

        private void StopMusic(AudioData audioData)
        {
            StartCoroutine(SmoothSwitchMusic(audioData, false));
        }

        private void StopSound(AudioData audioData) => 
            _soundsSource.Stop();

        private IEnumerator SmoothSwitchMusic(AudioData audioData, bool play)
        {
            var startVolume = _musicSource.volume;
            var targetVolume = play ? audioData.Volume : 0f;
            var t = 0f;

            if (play)
            {
                while (_musicSource.volume < targetVolume)
                {
                    LerpVolume();
                    yield return null;
                }

                if (!_musicSource.isPlaying) _musicSource.Play();
            }
            else
            {
                while (_musicSource.volume > targetVolume)
                {
                    LerpVolume();
                    yield return null;
                }

                _musicSource.Stop();
            }

            void LerpVolume()
            {
                _musicSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
                t += Time.deltaTime / SwitchTransitionTime;
            }
        }
    }
}