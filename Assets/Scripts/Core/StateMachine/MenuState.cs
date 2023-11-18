using System.Threading.Tasks;
using Core.AssetManagement.LocalAssetProviders;
using UI.Views;

namespace Core.StateMachine
{
    public class MenuState : ISimpleState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly LoadingScreenProvider _loadingScreenProvider;

        private MenuScreenProvider _menuScreenProvider;
        private MenuScreenView MenuView => _menuScreenProvider.LoadedObject.View;

        public MenuState(GameStateMachine stateMachine, LoadingScreenProvider loadingScreenProvider)
        {
            _stateMachine = stateMachine;
            _loadingScreenProvider = loadingScreenProvider;
        }

        public async void Enter()
        {
            await LoadMenuScreen();
            MenuView.PlayButton.onClick.AddListener(LoadGame);
            _loadingScreenProvider.TryUnload();
        }

        public void Exit()
        {
            _menuScreenProvider.TryUnload();
        }
        
        private async Task LoadMenuScreen()
        {
            _menuScreenProvider = new MenuScreenProvider();
            var loadTask = _menuScreenProvider.Load();
            await loadTask;
        }
        
        private void LoadGame()
        {
            MenuView.PlayButton.onClick.RemoveListener(LoadGame);
            _stateMachine.Enter<LoadSceneState, string>(AssetPath.GameScene);
        }
    }
}