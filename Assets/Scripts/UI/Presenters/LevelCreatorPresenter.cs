using LevelData;
using Main;
using UI.Views;
using UnityEngine;

namespace UI.Presenters
{
    public class LevelCreatorPresenter : MonoBehaviour
    {
        [SerializeField] private LevelCreatorView _view;
        [SerializeField] private LevelCreator _levelCreator;

        public LevelCreatorView View => _view;
        
        private void Start()
        {
            _view.EasyLevel.onClick.AddListener(GenerateEasyLevel);
            _view.MediumLevel.onClick.AddListener(GenerateMediumLevel);
            _view.HardLevel.onClick.AddListener(GenerateHardLevel);
            _view.SuperHardLevel.onClick.AddListener(GenerateSuperHardLevel);
            _view.ClearButton.onClick.AddListener(_levelCreator.ClearLevel);
        }

        private void OnDestroy()
        {
            _view.EasyLevel.onClick.RemoveListener(GenerateEasyLevel);
            _view.MediumLevel.onClick.RemoveListener(GenerateMediumLevel);
            _view.HardLevel.onClick.RemoveListener(GenerateHardLevel);
            _view.SuperHardLevel.onClick.RemoveListener(GenerateSuperHardLevel);
            _view.ClearButton.onClick.RemoveListener(_levelCreator.ClearLevel);
        }

        private void GenerateEasyLevel() => _levelCreator.GenerateLevel(LevelType.Easy);
        private void GenerateMediumLevel() => _levelCreator.GenerateLevel(LevelType.Medium);
        private void GenerateHardLevel() => _levelCreator.GenerateLevel(LevelType.Hard);
        private void GenerateSuperHardLevel() => _levelCreator.GenerateLevel(LevelType.SuperHard);
    }
}