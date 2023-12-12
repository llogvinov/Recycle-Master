using System.Threading.Tasks;
using Core.AssetManagement.LocalAssetProviders;
using Core.Data;
using Core.SaveService;
using UI.Base;

namespace Core.StateMachine
{
    public class GameOverState : IPayloadState<bool>
    {
        private const int MillisecondsPerSeconds = 1000;
        private const float Additional = 0.5f;
        
        private readonly GameStateMachine _stateMachine;
        private readonly ISaveService<PlayerProgressService> _saveService;
        private readonly UILoadingProvider _uiLoadingProvider;

        private UIWinLevelProvider _uiWinLevel;
        private UILostLevelProvider _uiLostLevel;

        public GameOverState(GameStateMachine stateMachine, 
            ISaveService<PlayerProgressService> saveService,
            UILoadingProvider uiLoadingProvider)
        {
            _stateMachine = stateMachine;
            _saveService = saveService;
            _uiLoadingProvider = uiLoadingProvider;
        }

        public async void Enter(bool won)
        {
            if (won)
                await PrepareUIWinLevel();
            else 
                await PrepareUILostLevel();
        }

        public void Exit()
        {
            if (_uiWinLevel is not null)
            {
                _uiWinLevel.LoadedObject.MenuButton.onClick.RemoveAllListeners();
                _uiWinLevel.LoadedObject.NextButton.onClick.RemoveAllListeners();
                _uiWinLevel.TryUnload();
                _uiWinLevel = null;
            }

            if (_uiLostLevel is not null)
            {
                _uiLostLevel.LoadedObject.MenuButton.onClick.RemoveAllListeners();
                _uiLostLevel.LoadedObject.RestartButton.onClick.RemoveAllListeners();
                _uiLostLevel.TryUnload();
                _uiLostLevel = null;
            }
        }

        private async Task PrepareUIWinLevel()
        {
            await LoadUIWinLevel();
            
            _uiWinLevel.LoadedObject.MenuButton.onClick.AddListener(LoadMenu);
            _uiWinLevel.LoadedObject.NextButton.onClick.AddListener(LoadNextLevel);
            
            _uiWinLevel.LoadedObject.Open();

            async Task LoadUIWinLevel()
            {
                _uiWinLevel = new UIWinLevelProvider();
                await _uiWinLevel.Load(disableOnInit: true);
            }

            async void LoadNextLevel()
            {
                _uiWinLevel.LoadedObject.Close();
                await Task.Delay((int)(UIPanel.AnimationDuration + Additional) * MillisecondsPerSeconds);
                await LoadUILoading();
                _stateMachine.Enter<PrepareGameState, int>(++_saveService.SaveData.CurrentLevel);
            }
        }

        private async Task PrepareUILostLevel()
        {
            await LoadUILostLevel();
            
            _uiLostLevel.LoadedObject.MenuButton.onClick.AddListener(LoadMenu);
            _uiLostLevel.LoadedObject.RestartButton.onClick.AddListener(RestartLevel);
            
            _uiLostLevel.LoadedObject.Open();
            
            async Task LoadUILostLevel()
            {
                _uiLostLevel = new UILostLevelProvider();
                await _uiLostLevel.Load(disableOnInit: true);
            }
            
            async void RestartLevel()
            {
                _uiLostLevel.LoadedObject.Close();
                await Task.Delay((int)(UIPanel.AnimationDuration + Additional) * MillisecondsPerSeconds);
                await LoadUILoading();
                _stateMachine.Enter<PrepareGameState, int>(_saveService.SaveData.CurrentLevel);
            }
        }
        
        private async Task LoadUILoading() => 
            await _uiLoadingProvider.Load();

        private void LoadMenu() => 
            _stateMachine.Enter<LoadSceneState, string>(AssetPath.MenuScene);
    }
}