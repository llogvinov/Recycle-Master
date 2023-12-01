using System.Threading.Tasks;
using Core.AssetManagement.LocalAssetProviders;
using Main;

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
            await Task.Delay(4000);
            _stateMachine.Enter<GameOverState, bool>(false);
        }

        public void Exit()
        {
            
        }
    }
}