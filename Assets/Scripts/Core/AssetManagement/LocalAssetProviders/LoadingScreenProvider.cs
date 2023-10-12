using System.Threading.Tasks;
using UI.Views;

namespace Core.AssetManagement.LocalAssetProviders
{
    public class LoadingScreenProvider : LocalAssetLoader<LoadingScreenView>
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