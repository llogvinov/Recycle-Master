using System.Threading.Tasks;
using UI;

namespace Core.AssetManagement.LocalAssetProviders
{
    public class UILoadingProvider : LocalAssetLoader<UILoadingScreen>
    {
        /// <summary>
        /// Delay before unloading UI screen (in milliseconds)
        /// </summary>
        private const int UnloadDelay = 500;
        
        protected override string AssetId => AddressablesLoadKeys.UILoading;
        
        protected override async void Unload()
        {
            await Task.Delay(UnloadDelay);
            base.Unload();
        }
    }
}