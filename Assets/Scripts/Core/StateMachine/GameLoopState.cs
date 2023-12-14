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
            
        }

        public void Exit()
        {
            
        }
    }
}