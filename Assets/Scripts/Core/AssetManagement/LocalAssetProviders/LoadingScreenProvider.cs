using System.Threading.Tasks;
using UI.Presenters;

namespace Core.AssetManagement.LocalAssetProviders
{
    public class LoadingScreenProvider : LocalAssetLoader<LoadingScreenPresenter>
    {
        /// <summary>
        /// Delay before unloading UI screen (in milliseconds)
        /// </summary>
        private const int UnloadDelay = 500;
        
        protected override string AssetId => AddressablesLoadKeys.LoadingScreen;
        
        protected override async void Unload()
        {
            await Task.Delay(UnloadDelay);
            base.Unload();
        }
    }
}