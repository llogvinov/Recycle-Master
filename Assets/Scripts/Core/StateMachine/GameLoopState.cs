namespace Core.StateMachine
{
    public class GameLoopState : ISimpleState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly Game _game;

        public GameLoopState(GameStateMachine stateMachine, Game game)
        {
            _stateMachine = stateMachine;
            _game = game;
        }

        public void Enter()
        {
            _game.GameOver += OnGameOver;
        }

        private void OnGameOver(bool won)
        {
            _stateMachine.Enter<GameOverState, bool>(won);
        }

        public void Exit()
        {
            _game.GameOver -= OnGameOver;
        }
    }
}