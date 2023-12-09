using System;
using System.Threading.Tasks;
using Core.AssetManagement.LocalAssetProviders;
using LevelData;
using Main;
using UI;
using UnityEngine;
using Random = System.Random;

namespace Core.StateMachine
{
    public class PrepareGameState : IPayloadState<int>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly Game _game;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly UILoadingProvider _uiLoadingProvider;

        private LevelCreator _levelCreator;
        private UITimerProvider _uiTimerProvider;

        public PrepareGameState(GameStateMachine gameStateMachine,
            Game game,
            ICoroutineRunner coroutineRunner,
            UILoadingProvider uiLoadingProvider)
        {
            _gameStateMachine = gameStateMachine;
            _game = game;
            _coroutineRunner = coroutineRunner;
            _uiLoadingProvider = uiLoadingProvider;
        }

        public async void Enter(int level)
        {
            Debug.Log($"current level - {level}");
            _levelCreator = GameObject.FindObjectOfType<LevelCreator>();
            await PrepareUITimer();
            BuildLevel();
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
            _uiLoadingProvider.TryUnload();
        }

        private async Task PrepareUITimer()
        {
            if (GameObject.FindObjectOfType<UITimer>() is not null) return;
            
            await LoadUITimer();
            Timer.Initialize(_coroutineRunner, _uiTimerProvider.LoadedObject);
            Timer.Instance.OnFinish += OnTimerFinished;

            async Task LoadUITimer()
            {
                _uiTimerProvider = new UITimerProvider();
                await _uiTimerProvider.Load();
            }

            void OnTimerFinished() => 
                _game.GameOver?.Invoke(false);
        }

        // todo: change level type choosing
        private void BuildLevel() => 
            BuildEasyLevel();

        private void BuildRandomLevel()
        {
            var values = Enum.GetValues(typeof(LevelType));
            var random = new Random();
            var randomType = (LevelType)values.GetValue(random.Next(values.Length));
            
            // exclude Undefined type
            if (randomType == 0) randomType++; 
            
            _levelCreator.GenerateLevel(randomType);
        }
        
#if UNITY_EDITOR
        private void BuildEasyLevel() => _levelCreator.GenerateLevel(LevelType.Easy);
        private void BuildMediumLevel() => _levelCreator.GenerateLevel(LevelType.Medium);
        private void BuildHardLevel() => _levelCreator.GenerateLevel(LevelType.Hard);
        private void BuildSuperHardLevel() => _levelCreator.GenerateLevel(LevelType.SuperHard);
#endif
    }
}