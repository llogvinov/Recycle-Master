using Core.AssetManagement.LocalAssetProviders;

namespace Core.StateMachine
{
    public class PrepareGameState : ISimpleState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly LoadingScreenProvider _loadingScreenProvider;

        public PrepareGameState(GameStateMachine gameStateMachine, LoadingScreenProvider loadingScreenProvider)
        {
            _gameStateMachine = gameStateMachine;
            _loadingScreenProvider = loadingScreenProvider;
        }

        public void Enter()
        {
            // todo: prepare game objects
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
            _loadingScreenProvider.TryUnload();
        }
    }
}