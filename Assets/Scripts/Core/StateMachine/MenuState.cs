using Core.AssetManagement.LocalAssetProviders;
using UI;
using UnityEngine;

namespace Core.StateMachine
{
    public class MenuState : ISimpleState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly UILoadingProvider _uiLoadingProvider;

        private UIMenu _uiMenu;

        public MenuState(GameStateMachine stateMachine, UILoadingProvider uiLoadingProvider)
        {
            _stateMachine = stateMachine;
            _uiLoadingProvider = uiLoadingProvider;
        }

        public void Enter()
        {
            _uiMenu = GameObject.FindObjectOfType<UIMenu>();
            _uiMenu.PlayButton.onClick.AddListener(LoadGame);
            _uiLoadingProvider.TryUnload();
        }

        public void Exit()
        {
            
        }
        
        private void LoadGame()
        {
            _uiMenu.PlayButton.onClick.RemoveListener(LoadGame);
            _stateMachine.Enter<LoadSceneState, string>(AssetPath.GameScene);
        }
    }
}