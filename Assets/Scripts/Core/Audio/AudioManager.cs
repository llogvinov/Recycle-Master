using System.Collections;
using System.ComponentModel;
using Core.Data;
using Core.SaveService;
using UnityEngine;

namespace Core.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundsSource;
        [Space] 
        [SerializeField] private AudioPlayer _musicPlayer;

        private PlayerSettingsData SaveData =>
            AllServices.Container.Single<ISaveService<PlayerSettingsData>>().SaveData;
        
        private const float SwitchTransitionTime = 0.5f;

        public AudioPlayer MusicPlayer => _musicPlayer;

        #region Singleton

        private static AudioManager _instance = null;

        public static AudioManager Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = FindObjectOfType<AudioManager>();
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }

        public static bool HasInstance => _instance is not null;

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
            if (SaveData.PlayMusic == false) return;
            _musicSource.clip = audioData.Clip;
            _musicSource.volume = audioData.Volume;
            StartCoroutine(SmoothSwitchMusic(audioData, true));
        }

        private void PlaySound(AudioData audioData)
        {
            if (SaveData.PlaySounds == false) return;
            _soundsSource.PlayOneShot(audioData.Clip, audioData.Volume);
        }

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

        private void StopMusic(AudioData audioData) => 
            StartCoroutine(SmoothSwitchMusic(audioData, false));

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