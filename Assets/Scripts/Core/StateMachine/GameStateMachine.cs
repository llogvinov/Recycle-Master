using System.Collections.Generic;
using System.Linq;
using Core.AssetManagement.LocalAssetProviders;
using Core.Data;
using Core.SaveService;

#if UNITY_EDITOR
using UnityEngine;
#endif

namespace Core.StateMachine
{
    public class GameStateMachine
    {
        private readonly List<IState> _states;
        private readonly Game _game;
        private readonly ICoroutineRunner _coroutineRunner;

        private IState _activeState;
        
        public GameStateMachine(Game game,
            AllServices services,
            ICoroutineRunner coroutineRunner,
            SceneLoader sceneLoader,
            UILoadingProvider uiLoadingProvider)
        {
            _game = game;
            _coroutineRunner = coroutineRunner;
            _states = new List<IState>
            {
                new BootstrapState(this, services),
                new TutorialState(this, _game, _coroutineRunner, services.Single<ISaveService<PlayerProgressData>>()),
                new MenuState(this, uiLoadingProvider),
                new LoadSceneState(this, services.Single<ISaveService<PlayerProgressData>>(), sceneLoader, uiLoadingProvider),
                new PrepareGameState(this, _game, _coroutineRunner, uiLoadingProvider),
                new GameLoopState(this, _game),
                new GameOverState(this, services.Single<ISaveService<PlayerProgressData>>(), uiLoadingProvider),
            };
        }

        public void Enter<TState>() where TState : class, ISimpleState
        {
#if UNITY_EDITOR
            Debug.Log($"enter {typeof(TState)} state");
#endif
            var state = ChangeState<TState>();
            state.Enter();
        }
        
        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
#if UNITY_EDITOR
            Debug.Log($"enter {typeof(TState)} state");
#endif
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