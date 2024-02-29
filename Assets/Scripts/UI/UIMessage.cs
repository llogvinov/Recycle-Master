using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIMessage : UIPanel
    {
        public Button SkipButton;
        
        [SerializeField] private Text _message;

        public void SetMessage(string message) => 
            _message.text = message;
    }
}