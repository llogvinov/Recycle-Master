﻿using System.Collections.Generic;
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
        
        public GameStateMachine(Game game, SceneLoader sceneLoader, LoadingScreenProvider loadingScreenProvider)
        {
            _game = game;
            _states = new List<IState>
            {
                new BootstrapState(this),
                new MenuState(this),
                new LoadSceneState(this, sceneLoader, loadingScreenProvider),
                new GameLoopState(this),
                new GameOverState(this),
            };
        }

        public void Enter<TState>() where TState : class, ISimpleState
        {
            Debug.Log($"enter {typeof(TState)} state");
            var state = ChangeState<TState>();
            state.Enter();
        }
        
        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            Debug.Log($"enter {typeof(TState)} state");
            var state = ChangeState<TState>();
            state.Enter(payload);
        }
        
        private TState ChangeState<TState>() where TState : class, IState
        {
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IState 
            => _states.FirstOrDefault(s => s is TState) as TState;
    }
}