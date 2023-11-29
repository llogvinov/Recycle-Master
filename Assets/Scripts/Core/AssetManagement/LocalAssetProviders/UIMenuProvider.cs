using UI;

namespace Core.AssetManagement.LocalAssetProviders
{
    public class UIMenuProvider : LocalAssetLoader<UIMenu>
    {
        protected override string AssetId => AddressablesLoadKeys.UIMenu;
    }
}