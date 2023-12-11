using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UILeave : UIScreen
    {
        [SerializeField] private Button _leaveButton;

        public Button LeaveButton => _leaveButton;
    }
}