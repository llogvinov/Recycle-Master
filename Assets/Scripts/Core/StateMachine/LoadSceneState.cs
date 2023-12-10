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
        private readonly ISaveService<PlayerProgressService> _saveService;
        private readonly UILoadingProvider _uiLoadingProvider;

        private string _loadingScene;

        public LoadSceneState(GameStateMachine stateMachine, 
            ISaveService<PlayerProgressService> saveService, 
            SceneLoader sceneLoader,
            UILoadingProvider uiLoadingProvider)
        {
            _stateMachine = stateMachine;
            _saveService = saveService;
            _sceneLoader = sceneLoader;
            _uiLoadingProvider = uiLoadingProvider;
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
        
        private async Task LoadUILoading()
        {
            var loadTask = _uiLoadingProvider.Load();
            await loadTask;
        }

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
            var currentLevel = _saveService.SaveData.CurrentLevel;
            _stateMachine.Enter<PrepareGameState, int>(currentLevel);
        }

        private void OnMenuSceneLoaded()
        {
            _stateMachine.Enter<MenuState>();
        }
    }
}