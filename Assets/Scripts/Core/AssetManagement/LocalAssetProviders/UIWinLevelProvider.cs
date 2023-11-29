using UI;

namespace Core.AssetManagement.LocalAssetProviders
{
    public class UIWinLevelProvider : LocalAssetLoader<UIWinLevel>
    {
        protected override string AssetId => AddressablesLoadKeys.UIWinLevel;
    }
}