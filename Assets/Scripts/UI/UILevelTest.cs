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

        private ISaveService<PlayerProgressData> _saveService;
        private PlayerProgressData _playerProgressData;
        
        private void Start()
        {
            _saveService = new BinarySaveService<PlayerProgressData>();

            CreateStartDate();
            UpdateUI();
            
            _plusButton.onClick.AddListener(() =>
            {
                _playerProgressData.CurrentLevel++;
                UpdateUI();
            });
            
            _minusButton.onClick.AddListener(() =>
            {
                _playerProgressData.CurrentLevel--;
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
            _playerProgressData = new PlayerProgressData()
            {
                CurrentLevel = 0
            };
        }

        private void UpdateUI()
        {
            _title.text = $"LEVEL {_playerProgressData.CurrentLevel}";
        }

        private void OnSaveButtonClicked()
        {
            _saveService.Save(_playerProgressData);
        }

        private void OnLoadButtonClicked()
        {
            _playerProgressData = _saveService.Load();
            UpdateUI();
        }
    }
}