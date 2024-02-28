using Core;
using Core.Audio;
using Core.Data;
using Core.SaveService;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UISettings : UIPanel
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _closeButton;
        [Space]
        [SerializeField] private Button _resetDataButton;
        [SerializeField] private Button _skipTutorialButton;
        [SerializeField] private Button _musicButton;
        [SerializeField] private Button _soundsButton;

        private void Start()
        {
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _resetDataButton.onClick.AddListener(OnResetButtonClicked);
            _skipTutorialButton.onClick.AddListener(OnSkipTutorialButtonClicked);
            _musicButton.onClick.AddListener(OnMusicButtonClicked);
            _soundsButton.onClick.AddListener(OnSoundsButtonClicked);
        }

        private void OnDestroy()
        {
            _settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            _resetDataButton.onClick.RemoveListener(OnResetButtonClicked);
            _skipTutorialButton.onClick.RemoveListener(OnSkipTutorialButtonClicked);
            _musicButton.onClick.RemoveListener(OnMusicButtonClicked);
            _soundsButton.onClick.RemoveListener(OnSoundsButtonClicked);
        }

        private void OnSettingsButtonClicked()
        {
            base.Open();
            _settingsButton.gameObject.SetActive(false);
        }

        private void OnCloseButtonClicked()
        {
            base.Close(() => _settingsButton.gameObject.SetActive(true));
        }

        private void OnSkipTutorialButtonClicked()
        {
            AllServices.Container.Single<ISaveService<PlayerProgressData>>().SaveData.TutorialCompleted = true;
            AllServices.Container.Single<ISaveService<PlayerProgressData>>().Save();
        }

        private void OnResetButtonClicked()
        {
            AllServices.Container.Single<ISaveService<PlayerProgressData>>().Clear();
            AllServices.Container.Single<ISaveService<PlayerSettingsData>>().Clear();
        }

        private void OnMusicButtonClicked()
        {
            var saveService = AllServices.Container.Single<ISaveService<PlayerSettingsData>>();
            var data = saveService.Load();
            data.PlayMusic = !data.PlayMusic;
            AudioManager.Instance.MusicPlayer.Switch(play: data.PlayMusic);
            saveService.Save();
        }

        private void OnSoundsButtonClicked()
        {
            var saveService = AllServices.Container.Single<ISaveService<PlayerSettingsData>>();
            var data = saveService.Load();
            data.PlaySounds = !data.PlaySounds;
            saveService.Save();
        }
    }
}