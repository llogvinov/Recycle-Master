using UI.Presenters;

namespace Core.AssetManagement.LocalAssetProviders
{
    public class MenuScreenProvider : LocalAssetLoader<MenuScreenPresenter>
    {
        protected override string AssetId => AddressablesLoadKeys.MenuScreen;
    }
}