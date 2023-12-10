using UI;

namespace Core.AssetManagement.LocalAssetProviders
{
    public class UITimerProvider : LocalAssetLoader<UITimer>
    {
        protected override string AssetId => AddressablesLoadKeys.UITimer;
    }
}