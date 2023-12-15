using Main;
using Main.Level;

namespace Core.StateMachine
{
    public class GameLoopState : IPayloadState<LevelManager>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly Game _game;

        public GameLoopState(GameStateMachine stateMachine, Game game)
        {
            _stateMachine = stateMachine;
            _game = game;
        }

        public void Enter(LevelManager levelManager)
        {
            Timer.Instance.StartCountdown(levelManager.LevelDifficultyData.CountdownTime);
        }

        public void Exit()
        {
            Timer.Instance.StopCountdown();
        }
    }
}