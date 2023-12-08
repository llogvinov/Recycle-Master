using Core.Data;
using Core.SaveService;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UILevelTest : MonoBehaviour
    {
        [SerializeField] private Text _title;
        [Space]
        [SerializeField] private Button _plusButton;
        [SerializeField] private Button _minusButton;
        [Space]
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadButton;

        private ISaveSystem _saveSystem;
        private PlayerProgress _playerProgress;
        
        private void Start()
        {
            _saveSystem = new BinarySaveSystem();

            CreateStartDate();
            UpdateUI();
            
            _plusButton.onClick.AddListener(() =>
            {
                _playerProgress.CurrentLevel++;
                UpdateUI();
            });
            
            _minusButton.onClick.AddListener(() =>
            {
                _playerProgress.CurrentLevel--;
                UpdateUI();
            });
            
            _saveButton.onClick.AddListener(OnSaveButtonClicked);
            _loadButton.onClick.AddListener(OnLoadButtonClicked);
        }

        private void OnDestroy()
        {
            _plusButton.onClick.RemoveAllListeners();
            _minusButton.onClick.RemoveAllListeners();
            _saveButton.onClick.RemoveAllListeners();
            _loadButton.onClick.RemoveAllListeners();
        }

        private void CreateStartDate()
        {
            _playerProgress = new PlayerProgress()
            {
                CurrentLevel = 0
            };
        }

        private void UpdateUI()
        {
            _title.text = $"LEVEL {_playerProgress.CurrentLevel}";
        }

        private void OnSaveButtonClicked()
        {
            _saveSystem.Save(_playerProgress);
        }

        private void OnLoadButtonClicked()
        {
            _playerProgress = _saveSystem.Load();
            UpdateUI();
        }
    }
}