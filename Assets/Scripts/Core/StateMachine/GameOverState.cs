using System.Threading.Tasks;
using Core.AssetManagement.LocalAssetProviders;
using Core.Data;
using Core.SaveService;
using Main;
using Main.Level;
using UI;
using UI.Base;
using UnityEngine;

namespace Core.StateMachine
{
    public class GameOverState : IPayloadState<GameOverCondition>
    {
        private const int MillisecondsPerSeconds = 1000;
        private const float Additional = 0.5f;
        
        private readonly GameStateMachine _stateMachine;
        private readonly ISaveService<PlayerProgressData> _saveService;
        private readonly UILoadingProvider _uiLoadingProvider;

        private readonly UIWinLevelProvider _uiWinLevelProvider;
        private readonly UILostLevelProvider _uiLostLevelProvider;

        private UIWinLevel UIWinLvl => _uiWinLevelProvider.LoadedObject;
        private UILostLevel UILostLvl => _uiLostLevelProvider.LoadedObject;

        public GameOverState(GameStateMachine stateMachine, 
            ISaveService<PlayerProgressData> saveService,
            UILoadingProvider uiLoadingProvider)
        {
            _stateMachine = stateMachine;
            _saveService = saveService;
            _uiLoadingProvider = uiLoadingProvider;
            _uiWinLevelProvider = new UIWinLevelProvider();
            _uiLostLevelProvider = new UILostLevelProvider();
        }

        public async void Enter(GameOverCondition condition)
        {
            switch (condition)
            {
                case GameOverCondition.Won:
                    UpdateSaveData();
                    await PrepareUIWinLevel();
                    await CacheNextLevel();
                    break;
                case GameOverCondition.LostByTime:
                    await PrepareUILostLevel();
                    break;
                case GameOverCondition.Left:
                    LoadMenu();
                    break;
                case GameOverCondition.TutorialCompleted:
                    await CacheNextLevel();
                    _stateMachine.Enter<LoadSceneState, string>(AssetPath.GameScene);
                    break;
            }
        }

        public void Exit()
        {
            if (_uiWinLevelProvider.HasLoadedObject)
            {
                UIWinLvl.MenuButton.onClick.RemoveAllListeners();
                UIWinLvl.NextButton.onClick.RemoveAllListeners();
                _uiWinLevelProvider.TryUnload();
            }

            if (_uiLostLevelProvider.HasLoadedObject)
            {
                UILostLvl.MenuButton.onClick.RemoveAllListeners();
                UILostLvl.RestartButton.onClick.RemoveAllListeners();
                _uiLostLevelProvider.TryUnload();
            }
        }

        private void UpdateSaveData()
        {
            _saveService.SaveData.CurrentLevel++;
            _saveService.Save();
        }

        private async Task PrepareUIWinLevel()
        {
            await LoadUIWinLevel();
            
            UIWinLvl.MenuButton.onClick.AddListener(LoadMenu);
            UIWinLvl.NextButton.onClick.AddListener(LoadNextLevel);
            
            UIWinLvl.Open();

            async Task LoadUIWinLevel() => 
                await _uiWinLevelProvider.Load(disableOnInit: true);

            async void LoadNextLevel()
            {
                UIWinLvl.Close();
                await Task.Delay((int)(UIPanel.AnimationDuration + Additional) * MillisecondsPerSeconds);
                await LoadUILoading();
                _stateMachine.Enter<PrepareGameState, int>(_saveService.SaveData.CurrentLevel);
            }
        }

        private async Task CacheNextLevel() => 
            await CachedLevel.CacheLevel(_saveService.SaveData.CurrentLevel);

        private async Task PrepareUILostLevel()
        {
            await LoadUILostLevel();
            
            UILostLvl.MenuButton.onClick.AddListener(LoadMenu);
            UILostLvl.RestartButton.onClick.AddListener(RestartLevel);
            
            UILostLvl.Open();
            
            async Task LoadUILostLevel() => 
                await _uiLostLevelProvider.Load(disableOnInit: true);

            async void RestartLevel()
            {
                UILostLvl.Close();
                await Task.Delay((int)(UIPanel.AnimationDuration + Additional) * MillisecondsPerSeconds);
                await LoadUILoading();
                _stateMachine.Enter<PrepareGameState, int>(_saveService.SaveData.CurrentLevel);
            }
        }
        
        private async Task LoadUILoading() => 
            await _uiLoadingProvider.Load();

        private void LoadMenu()
        {
            Timer.ClearInstance();
            _stateMachine.Enter<LoadSceneState, string>(AssetPath.MenuScene);
        }
    }
}