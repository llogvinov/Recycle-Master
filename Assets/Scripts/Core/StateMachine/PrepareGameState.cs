using System.Threading.Tasks;
using Core.AssetManagement.LocalAssetProviders;
using LevelData;
using Main;
using Main.Level;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.StateMachine
{
    public class PrepareGameState : IPayloadState<int>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly Game _game;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly UILoadingProvider _uiLoadingProvider;

        private LevelManager _levelManager;
        private UITimerProvider _uiTimerProvider;

        private UITimer UITimer => _uiTimerProvider.LoadedObject;

        public PrepareGameState(GameStateMachine stateMachine,
            Game game,
            ICoroutineRunner coroutineRunner,
            UILoadingProvider uiLoadingProvider)
        {
            _stateMachine = stateMachine;
            _game = game;
            _coroutineRunner = coroutineRunner;
            _uiLoadingProvider = uiLoadingProvider;
        }

        public async void Enter(int level)
        {
            Debug.Log($"current level - {level}");

            _game.GameOver = null;
            _game.GameOver += (condition) =>  _stateMachine.Enter<GameOverState, GameOverCondition>(condition);

            GameObject.FindObjectOfType<UIMessage>().Close();
            PrepareLevelManager();
            BuildLevel();
            await PrepareUITimer();
            PrepareUIPause();

            _stateMachine.Enter<GameLoopState, LevelManager>(_levelManager);
        }

        public void Exit()
        {
            _uiLoadingProvider.TryUnload();
        }

        private void PrepareLevelManager()
        {
            _levelManager = GameObject.FindObjectOfType<LevelManager>();
            if (_levelManager is not null)
            {
                _levelManager.LevelComplete.RemoveAllListeners();
                _levelManager.LevelComplete.AddListener(() => 
                    _game.GameOver(GameOverCondition.Won));
            }
        }
        
        private void BuildLevel()
        {
            if (CachedLevel.CurrentLevelDetailsData is not null)
                _levelManager.BuildCurrentLevel();
            else
                _levelManager.BuildRandomLevel(GetRandomLevelType());
        }

        private async Task PrepareUITimer()
        {
            if (GameObject.FindObjectOfType<UITimer>() is null)
            {
                await LoadUITimer();
                Timer.Initialize(_coroutineRunner, UITimer);
            }

            Timer.Instance.OnFinish = null;
            Timer.Instance.OnFinish += OnTimerFinished;

            async Task LoadUITimer()
            {
                _uiTimerProvider = new UITimerProvider();
                await _uiTimerProvider.Load();
            }

            void OnTimerFinished() => 
                _game.GameOver?.Invoke(GameOverCondition.LostByTime);
        }

        private void PrepareUIPause()
        {
            UIPause.PauseButtonClicked += OnPauseClicked;
            UIPause.LeaveButtonClicked += OnLeaveClicked;
            UIPause.ContinueButtonClicked += OnContinueClicked;
            
            void OnPauseClicked() => 
                Timer.PauseTimer();

            void OnLeaveClicked() => 
                _stateMachine.Enter<GameOverState, GameOverCondition>(GameOverCondition.Left);

            void OnContinueClicked() => 
                Timer.ContinueTimer();
        }

        /*private void BuildRandomLevel()
        {
            var values = Enum.GetValues(typeof(LevelType));
            var random = new Random();
            var randomType = (LevelType)values.GetValue(random.Next(values.Length));
            
            // exclude Undefined type
            if (randomType == 0) randomType++; 
            
            _levelManager.BuildRandomLevel(randomType);
        }*/

        private LevelType GetRandomLevelType()
        {
            var randomValue = Random.Range(0, 100);
            
            return randomValue switch
            {
                < 50 => LevelType.Easy,
                < 80 => LevelType.Medium,
                < 95 => LevelType.Hard,
                _ => LevelType.SuperHard
            };
        }
        
#if UNITY_EDITOR
        private void BuildEasyLevel() => _levelManager.BuildRandomLevel(LevelType.Easy);
        private void BuildMediumLevel() => _levelManager.BuildRandomLevel(LevelType.Medium);
        private void BuildHardLevel() => _levelManager.BuildRandomLevel(LevelType.Hard);
        private void BuildSuperHardLevel() => _levelManager.BuildRandomLevel(LevelType.SuperHard);
#endif
    }
}