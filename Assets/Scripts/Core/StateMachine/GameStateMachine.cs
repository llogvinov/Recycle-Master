using System.Collections.Generic;
using System.Linq;
using Core.AssetManagement.LocalAssetProviders;
using Core.Data;
using Core.SaveService;

namespace Core.StateMachine
{
    public class GameStateMachine
    {
        private readonly List<IState> _states;
        private readonly Game _game;
        
        private IState _activeState;
        
        public GameStateMachine(Game game, 
            AllServices services, 
            SceneLoader sceneLoader,
            UILoadingProvider uiLoadingProvider)
        {
            _game = game;
            _states = new List<IState>
            {
                new BootstrapState(this, services),
                new MenuState(this, uiLoadingProvider),
                new LoadSceneState(this, services.Single<ISaveService<PlayerProgressService>>(), sceneLoader, uiLoadingProvider),
                new PrepareGameState(this, _game, uiLoadingProvider),
                new GameLoopState(this, _game),
                new GameOverState(this, services.Single<ISaveService<PlayerProgressService>>()),
            };
        }

        public void Enter<TState>() where TState : class, ISimpleState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }
        
        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }
        
        private TState ChangeState<TState>() where TState : class, IState
        {
            _activeState?.Exit();
            var state = GetState<TState>();
            _activeState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IState 
            => _states.FirstOrDefault(s => s is TState) as TState;
    }
}