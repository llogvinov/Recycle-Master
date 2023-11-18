using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class MenuScreenView : MonoBehaviour
    {
        [SerializeField] private Button _playButton;

        public Button PlayButton => _playButton;
    }
}