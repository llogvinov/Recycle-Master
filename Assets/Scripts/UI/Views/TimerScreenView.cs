using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class TimerScreenView : MonoBehaviour
    {
        [SerializeField] private Text _timerText;

        public Text TimerText => _timerText;
    }
}
