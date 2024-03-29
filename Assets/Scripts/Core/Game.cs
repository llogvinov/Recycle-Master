﻿using System;
using Core.AssetManagement.LocalAssetProviders;
using Core.StateMachine;

namespace Core
{
    public class Game
    {
        private readonly GameStateMachine _stateMachine;
        public GameStateMachine StateMachine => _stateMachine;
        
        public Action<GameOverCondition> GameOver;

        public Game(ICoroutineRunner coroutineRunner)
        {
            _stateMachine = new GameStateMachine(this, 
                AllServices.Container,
                coroutineRunner,
                new SceneLoader(coroutineRunner), 
                new UILoadingProvider());
        }
    }

    public enum GameOverCondition
    {
        Won,
        LostByTime,
        Left,
        TutorialCompleted,
    }
}