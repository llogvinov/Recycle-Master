using Core.Tutorial.UI;

namespace Core.AssetManagement.LocalAssetProviders
{
    public class TutorialProvider : LocalAssetLoader<TutorialMessages>
    {
        protected override string AssetId => AddressablesLoadKeys.Tutorial;
    }
}