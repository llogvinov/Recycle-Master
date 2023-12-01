using System.Collections.Generic;
using System.Linq;
using Core.AssetManagement.LocalAssetProviders;
using UnityEngine;

namespace Core.StateMachine
{
    public class GameStateMachine
    {
        private readonly List<IState> _states;
        private readonly Game _game;
        
        private IState _activeState;
        
        public GameStateMachine(Game game, SceneLoader sceneLoader, UILoadingProvider uiLoadingProvider)
        {
            _game = game;
            _states = new List<IState>
            {
                new BootstrapState(this),
                new MenuState(this, uiLoadingProvider),
                new LoadSceneState(this, sceneLoader, uiLoadingProvider),
                new PrepareGameState(this, uiLoadingProvider),
                new GameLoopState(this),
                new GameOverState(this),
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