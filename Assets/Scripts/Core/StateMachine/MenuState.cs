using System.Threading.Tasks;
using Core.AssetManagement.LocalAssetProviders;
using UI;

namespace Core.StateMachine
{
    public class MenuState : ISimpleState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly UILoadingProvider _uiLoadingProvider;

        private UIMenuProvider _uiMenuProvider;
        private UIMenu UIMenu => _uiMenuProvider.LoadedObject;

        public MenuState(GameStateMachine stateMachine, UILoadingProvider uiLoadingProvider)
        {
            _stateMachine = stateMachine;
            _uiLoadingProvider = uiLoadingProvider;
        }

        public async void Enter()
        {
            await LoadUIMenu();
            UIMenu.PlayButton.onClick.AddListener(LoadGame);
            _uiLoadingProvider.TryUnload();
        }

        public void Exit()
        {
            _uiMenuProvider.TryUnload();
        }
        
        private async Task LoadUIMenu()
        {
            _uiMenuProvider = new UIMenuProvider();
            var loadTask = _uiMenuProvider.Load();
            await loadTask;
        }
        
        private void LoadGame()
        {
            UIMenu.PlayButton.onClick.RemoveListener(LoadGame);
            _stateMachine.Enter<LoadSceneState, string>(AssetPath.GameScene);
        }
    }
}