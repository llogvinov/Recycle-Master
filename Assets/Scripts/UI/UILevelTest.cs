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

        private ISaveService<PlayerProgressService> _saveService;
        private PlayerProgressService _playerProgressService;
        
        private void Start()
        {
            _saveService = new BinarySaveService<PlayerProgressService>();

            CreateStartDate();
            UpdateUI();
            
            _plusButton.onClick.AddListener(() =>
            {
                _playerProgressService.CurrentLevel++;
                UpdateUI();
            });
            
            _minusButton.onClick.AddListener(() =>
            {
                _playerProgressService.CurrentLevel--;
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
            _playerProgressService = new PlayerProgressService()
            {
                CurrentLevel = 0
            };
        }

        private void UpdateUI()
        {
            _title.text = $"LEVEL {_playerProgressService.CurrentLevel}";
        }

        private void OnSaveButtonClicked()
        {
            _saveService.Save(_playerProgressService);
        }

        private void OnLoadButtonClicked()
        {
            _playerProgressService = _saveService.Load();
            UpdateUI();
        }
    }
}