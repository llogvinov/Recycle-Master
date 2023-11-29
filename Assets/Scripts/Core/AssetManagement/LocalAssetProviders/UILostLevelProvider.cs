using UI;

namespace Core.AssetManagement.LocalAssetProviders
{
    public class UILostLevelProvider : LocalAssetLoader<UILostLevel>
    {
        protected override string AssetId => AddressablesLoadKeys.UILostLevel;
    }
}