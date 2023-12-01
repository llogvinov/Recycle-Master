using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIMenu : UIScreen
    {
        [SerializeField] private Button _playButton;

        public Button PlayButton => _playButton;
    }
}