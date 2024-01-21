using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIMessage : UIPanel
    {
        /*public Action MessageRead;
        public Action MessageSkiped;
        
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _skipButton;
        
        private void OnEnable()
        {
            _closeButton.onClick.AddListener(CloseMessage);
            _nextButton.onClick.AddListener(CloseMessage);
            _skipButton.onClick.AddListener(SkipPart);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(CloseMessage);
            _nextButton.onClick.RemoveListener(CloseMessage);
            _skipButton.onClick.RemoveListener(SkipPart);
        }

        private void SkipPart() => 
            base.Close(() => MessageSkiped?.Invoke());
        
        private void CloseMessage() => 
            base.Close(() => MessageRead?.Invoke());*/

        public Button SkipButton;
        [SerializeField] private Text _message;

        public void SetMessage(string message) => 
            _message.text = message;
    }
}