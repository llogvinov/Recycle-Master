using System.Threading.Tasks;
using Core.AssetManagement.LocalAssetProviders;

namespace Core.StateMachine
{
    public class LoadSceneState : IPayloadState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly UILoadingProvider _uiLoadingProvider;

        private string _loadingScene;

        public LoadSceneState(GameStateMachine stateMachine, SceneLoader sceneLoader, UILoadingProvider uiLoadingProvider)
        {
            _stateMachine = stateMachine;
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
            _stateMachine.Enter<PrepareGameState>();
        }

        private void OnMenuSceneLoaded()
        {
            _stateMachine.Enter<MenuState>();
        }
    }
}