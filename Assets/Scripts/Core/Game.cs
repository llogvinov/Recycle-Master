﻿using System;
using Core.AssetManagement.LocalAssetProviders;
using Core.StateMachine;

namespace Core
{
    public class Game
    {
        private readonly GameStateMachine _stateMachine;
        public GameStateMachine StateMachine => _stateMachine;

        public Action GameOver;

        public Game(ICoroutineRunner coroutineRunner)
        {
            _stateMachine = new GameStateMachine(this, 
                new SceneLoader(coroutineRunner), 
                new UILoadingProvider());
        }
    }
}