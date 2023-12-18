using System.Threading.Tasks;
using Core.AssetManagement.LocalAssetProviders;
using Core.Data;
using Core.SaveService;

namespace Core.StateMachine
{
    public class LoadSceneState : IPayloadState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ISaveService<PlayerProgressData> _progressDataSaveService;
        private readonly UILoadingProvider _uiLoadingProvider;
        
        private readonly PlayerProgressData _progressData;

        private string _loadingScene;

        public LoadSceneState(GameStateMachine stateMachine, 
            ISaveService<PlayerProgressData> progressDataSaveService, 
            SceneLoader sceneLoader,
            UILoadingProvider uiLoadingProvider)
        {
            _stateMachine = stateMachine;
            _progressDataSaveService = progressDataSaveService;
            _sceneLoader = sceneLoader;
            _uiLoadingProvider = uiLoadingProvider;
            
            _progressData = _progressDataSaveService.Load();
        }

        public async void Enter(string sceneName)
        {
            _loadingScene = sceneName;
            await LoadUILoading();
            _sceneLoader.LoadScene(sceneName, OnSceneLoaded);
        }

        public void Exit()
        {
            
        }
        
        private async Task LoadUILoading() => 
            await _uiLoadingProvider.Load();

        private void OnSceneLoaded()
        {
            switch (_loadingScene)
            {
                case AssetPath.GameScene:
                    OnGameSceneLoaded();
                    break;
                case AssetPath.MenuScene:
                    OnMenuSceneLoaded();
                    break;
            }
        }

        private void OnGameSceneLoaded()
        {
            if (_progressData.TutorialCompleted)
            {
                var currentLevel = _progressDataSaveService.SaveData.CurrentLevel;
                _stateMachine.Enter<PrepareGameState, int>(currentLevel);
            }
            else
            {
                _stateMachine.Enter<TutorialState>();
            }
        }

        private void OnMenuSceneLoaded()
        {
            _stateMachine.Enter<MenuState>();
        }
    }
}