using System.Collections;
using Core.Data;
using Core.SaveService;
using Main;
using Main.Level;
using ObjectsData;
using UnityEngine;

namespace Core.StateMachine
{
    public class TutorialState : ISimpleState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly Game _game;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ISaveService<PlayerProgressData> _playerProgressData;
        
        private LevelManager _levelManager;

        public TutorialState(GameStateMachine stateMachine, 
            Game game, 
            ICoroutineRunner coroutineRunner, 
            ISaveService<PlayerProgressData> playerProgressData)
        {
            _stateMachine = stateMachine;
            _game = game;
            _coroutineRunner = coroutineRunner;
            _playerProgressData = playerProgressData;
        }

        public void Enter()
        {
            PrepareLevelManager();
            _coroutineRunner.StartCoroutine(ProceedTutorial());

            _game.GameOver = null;
            _game.GameOver += (condition) =>  _stateMachine.Enter<GameOverState, GameOverCondition>(condition);
        }
        
        public void Exit()
        {
            _levelManager.LevelComplete = null;
        }

        private IEnumerator ProceedTutorial()
        {
            foreach (var trashCanData in ResourceLoader.TrashCanDatas)
            {
                var tutorialPartCompleted = false;
                _levelManager.LevelComplete = null;
                _levelManager.LevelComplete += () => tutorialPartCompleted = true;
                GenerateTutorialLevel(trashCanData);
                while (!tutorialPartCompleted)
                {
                    yield return new WaitForSeconds(1f);
                }
            }
            UpdateProgressData();
            _game.GameOver(GameOverCondition.Won);
        }

        private void PrepareLevelManager() => 
            _levelManager = GameObject.FindObjectOfType<LevelManager>();

        private void GenerateTutorialLevel(TrashCanData trashCanData) => 
            _levelManager.GenerateTutorialLevel(trashCanData);

        private void UpdateProgressData()
        {
            _playerProgressData.SaveData.TutorialCompleted = true;
            _playerProgressData.Save();
        }
    }
}