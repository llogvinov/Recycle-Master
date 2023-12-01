using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIWinLevel : UIPanel
    {
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _nextButton;

        public Button MenuButton => _menuButton;

        public Button NextButton => _nextButton;
    }
}