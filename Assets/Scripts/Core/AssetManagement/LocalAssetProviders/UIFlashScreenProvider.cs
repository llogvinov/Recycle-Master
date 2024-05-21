using UI;

namespace Core.AssetManagement.LocalAssetProviders
{
    public class UIFlashScreenProvider : LocalAssetLoader<UIFlashScreen>
    {
        protected override string AssetId => AddressablesLoadKeys.UIFlashScreen;
    }
}