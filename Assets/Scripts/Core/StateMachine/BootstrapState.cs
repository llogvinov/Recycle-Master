using Core.Data;
using Core.SaveService;

namespace Core.StateMachine
{
    public class BootstrapState : ISimpleState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, AllServices services)
        {
            _stateMachine = stateMachine;
            _services = services;
            
            RegisterServices();

            _services.Single<ISaveService<PlayerProgressService>>().Load();
        }

        public void Enter()
        {
            _stateMachine.Enter<MenuState>();
        }

        public void Exit()
        {
            
        }
        
        private void RegisterServices()
        {
            _services.RegisterSingle<ISaveService<PlayerProgressService>>(
                new BinarySaveService<PlayerProgressService>());
        }
    }
}