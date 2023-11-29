using LevelData;
using Main;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UILevelCreator : UIBase
    {
        [SerializeField] private Button _easyLevel;
        [SerializeField] private Button _mediumLevel;
        [SerializeField] private Button _hardLevel;
        [SerializeField] private Button _superHardLevel;
        [SerializeField] private Button _clearButton;
        [Space]
        [SerializeField] private LevelCreator _levelCreator;

        private void Start()
        {
            _easyLevel.onClick.AddListener(GenerateEasyLevel);
            _mediumLevel.onClick.AddListener(GenerateMediumLevel);
            _hardLevel.onClick.AddListener(GenerateHardLevel);
            _superHardLevel.onClick.AddListener(GenerateSuperHardLevel);
            _clearButton.onClick.AddListener(_levelCreator.ClearLevel);
        }

        private void OnDestroy()
        {
            _easyLevel.onClick.RemoveListener(GenerateEasyLevel);
            _mediumLevel.onClick.RemoveListener(GenerateMediumLevel);
            _hardLevel.onClick.RemoveListener(GenerateHardLevel);
            _superHardLevel.onClick.RemoveListener(GenerateSuperHardLevel);
            _clearButton.onClick.RemoveListener(_levelCreator.ClearLevel);
        }

        private void GenerateEasyLevel() => _levelCreator.GenerateLevel(LevelType.Easy);
        private void GenerateMediumLevel() => _levelCreator.GenerateLevel(LevelType.Medium);
        private void GenerateHardLevel() => _levelCreator.GenerateLevel(LevelType.Hard);
        private void GenerateSuperHardLevel() => _levelCreator.GenerateLevel(LevelType.SuperHard);
    }
}