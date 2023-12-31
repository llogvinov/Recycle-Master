using System.Threading.Tasks;
using Core.Audio;
using Core.Data;
using Core.SaveService;
using Main.Level;

namespace Core.StateMachine
{
    public class BootstrapState : ISimpleState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly AllServices _services;
        private readonly PlayerProgressData _progressData;
        private readonly PlayerSettingsData _settingsData;

        public BootstrapState(GameStateMachine stateMachine, AllServices services)
        {
            _stateMachine = stateMachine;
            _services = services;
            
            RegisterServices();

            _progressData = _services.Single<ISaveService<PlayerProgressData>>().Load();
            _settingsData = _services.Single<ISaveService<PlayerSettingsData>>().Load();
        }

        private async Task CacheCurrentLevel() => 
            await CachedLevel.CacheLevel(_progressData.CurrentLevel);

        public async void Enter()
        {
            AudioManager.Instance.MusicPlayer.Switch(_settingsData.PlayMusic);
            await CacheCurrentLevel();
            _stateMachine.Enter<MenuState>();
        }

        public void Exit()
        {
            
        }
        
        private void RegisterServices()
        {
            _services.RegisterSingle<ISaveService<PlayerProgressData>>(
                new BinarySaveService<PlayerProgressData>());
            _services.RegisterSingle<ISaveService<PlayerSettingsData>>(
                new BinarySaveService<PlayerSettingsData>());
        }
    }
}