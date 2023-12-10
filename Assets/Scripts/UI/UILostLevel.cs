using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UILostLevel : UIPanel
    {
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _restartButton;

        public Button MenuButton => _menuButton;

        public Button RestartButton => _restartButton;
    }
}