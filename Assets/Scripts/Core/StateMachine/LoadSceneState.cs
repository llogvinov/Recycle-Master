using System.Threading.Tasks;
using Core.AssetManagement.LocalAssetProviders;

namespace Core.StateMachine
{
    public class LoadSceneState : IPayloadState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingScreenProvider _loadingScreenProvider;

        private string _loadingScene;

        public LoadSceneState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingScreenProvider loadingScreenProvider)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingScreenProvider = loadingScreenProvider;
        }

        public async void Enter(string sceneName)
        {
            _loadingScene = sceneName;
            await LoadLoadingScreen();
            _sceneLoader.LoadScene(sceneName, OnSceneLoaded);
        }

        public void Exit()
        {
            
        }
        
        private async Task LoadLoadingScreen()
        {
            var loadTask = _loadingScreenProvider.Load();
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
            _stateMachine.Enter<GameLoopState>();
        }

        private void OnMenuSceneLoaded()
        {
            _stateMachine.Enter<MenuState>();
        }
    }
}