using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class LevelCreatorView : MonoBehaviour
    {
        [SerializeField] private Button _easyLevel;
        [SerializeField] private Button _mediumLevel;
        [SerializeField] private Button _hardLevel;
        [SerializeField] private Button _superHardLevel;
        [SerializeField] private Button _clearButton;

        public Button EasyLevel => _easyLevel;

        public Button MediumLevel => _mediumLevel;

        public Button HardLevel => _hardLevel;

        public Button SuperHardLevel => _superHardLevel;

        public Button ClearButton => _clearButton;
    }
}