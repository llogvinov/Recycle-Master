using Core.Data;
using Core.SaveService;
using Main;
using Main.Level;
using UnityEngine;

namespace Core.StateMachine
{
    public class TutorialState : ISimpleState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly Game _game;
        private readonly ISaveService<PlayerProgressData> _playerProgressData;
        
        private LevelManager _levelManager;

        public TutorialState(GameStateMachine stateMachine, Game game, ISaveService<PlayerProgressData> playerProgressData)
        {
            _stateMachine = stateMachine;
            _game = game;
            _playerProgressData = playerProgressData;
        }

        public void Enter()
        {
            PrepareLevelManager();
            _levelManager.GenerateTutorialLevel(ResourceLoader.TrashCanDatas[0]);
            
            _game.GameOver = null;
            _game.GameOver += (condition) =>  _stateMachine.Enter<GameOverState, GameOverCondition>(condition);
        }

        public void Exit()
        {
            
        }
        
        private void PrepareLevelManager()
        {
            _levelManager = GameObject.FindObjectOfType<LevelManager>();
            if (_levelManager is not null)
                _levelManager.Game = _game;
        }
    }
}