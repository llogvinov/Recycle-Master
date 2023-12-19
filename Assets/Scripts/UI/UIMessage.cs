using System;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIMessage : UIPanel
    {
        public Action MessageRead;
        public Action MessageSkiped;
        
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _skipButton;
        [SerializeField] private Text _title;
        [SerializeField] private Text _message;

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
            base.Close(() => MessageRead?.Invoke());

        public void SetMessage(string title, string message)
        {
            _title.text = title;
            _message.text = message;
        }
    }
}