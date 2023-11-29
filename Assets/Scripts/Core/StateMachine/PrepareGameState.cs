using System;
using System.Threading.Tasks;
using Core.AssetManagement.LocalAssetProviders;
using LevelData;
using Main;
using UnityEngine;
using Random = System.Random;

namespace Core.StateMachine
{
    public class PrepareGameState : ISimpleState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly UILoadingProvider _uiLoadingProvider;

        private LevelCreator _levelCreator;
        private UITimerProvider _uiTimerProvider;

        public PrepareGameState(GameStateMachine gameStateMachine, UILoadingProvider uiLoadingProvider)
        {
            _gameStateMachine = gameStateMachine;
            _uiLoadingProvider = uiLoadingProvider;
        }

        public async void Enter()
        {
            _levelCreator = GameObject.FindObjectOfType<LevelCreator>();
            await LoadUITimer();
            BuildLevel();
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
            _uiLoadingProvider.TryUnload();
        }

        private async Task LoadUITimer()
        {
            _uiTimerProvider = new UITimerProvider();
            var loadTask = _uiTimerProvider.Load();
            await loadTask;
        }

        // todo: change level type choosing
        private void BuildLevel() => 
            BuildRandomLevel();

        private void BuildRandomLevel()
        {
            var values = Enum.GetValues(typeof(LevelType));
            var random = new Random();
            var randomType = (LevelType)values.GetValue(random.Next(values.Length));
            
            _levelCreator.GenerateLevel(randomType);
        }
    }
}