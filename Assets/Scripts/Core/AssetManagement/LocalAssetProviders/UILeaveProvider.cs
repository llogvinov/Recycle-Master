using UI;

namespace Core.AssetManagement.LocalAssetProviders
{
    public class UILeaveProvider : LocalAssetLoader<UILeave>
    {
        protected override string AssetId => AddressablesLoadKeys.UILeave;
    }
}