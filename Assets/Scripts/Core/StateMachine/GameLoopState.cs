using System.Threading.Tasks;
using UI;
using UnityEngine;

namespace Core.StateMachine
{
    public class GameLoopState : ISimpleState
    {
        private readonly GameStateMachine _stateMachine;

        public GameLoopState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public async void Enter()
        {
            await Test();
        }

        private async Task Test()
        {
            await Task.Delay(2000);
            GameObject.FindObjectOfType<UITimer>().PauseTimer();
            await Task.Delay(4000);
            _stateMachine.Enter<GameOverState, bool>(false);
        }

        public void Exit()
        {
            
        }
    }
}