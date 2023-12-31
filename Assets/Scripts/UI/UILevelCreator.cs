﻿using LevelData;
using Main;
using Main.Level;
using UI.Base;
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
        [SerializeField] private Button _specificLevelButton;
        [Space]
        [SerializeField] private LevelManager _levelManager;

        private void Start()
        {
            _easyLevel.onClick.AddListener(GenerateEasyLevel);
            _mediumLevel.onClick.AddListener(GenerateMediumLevel);
            _hardLevel.onClick.AddListener(GenerateHardLevel);
            _superHardLevel.onClick.AddListener(GenerateSuperHardLevel);
            _clearButton.onClick.AddListener(_levelManager.ClearLevel);
            _specificLevelButton.onClick.AddListener(_levelManager.BuildSpecificLevel);
        }

        private void OnDestroy()
        {
            _easyLevel.onClick.RemoveListener(GenerateEasyLevel);
            _mediumLevel.onClick.RemoveListener(GenerateMediumLevel);
            _hardLevel.onClick.RemoveListener(GenerateHardLevel);
            _superHardLevel.onClick.RemoveListener(GenerateSuperHardLevel);
            _clearButton.onClick.RemoveListener(_levelManager.ClearLevel);
            _specificLevelButton.onClick.RemoveListener(_levelManager.BuildSpecificLevel);
        }

        private void GenerateEasyLevel() => _levelManager.BuildRandomLevel(LevelType.Easy);
        private void GenerateMediumLevel() => _levelManager.BuildRandomLevel(LevelType.Medium);
        private void GenerateHardLevel() => _levelManager.BuildRandomLevel(LevelType.Hard);
        private void GenerateSuperHardLevel() => _levelManager.BuildRandomLevel(LevelType.SuperHard);
    }
}