using System;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIMessage : UIPanel
    {
        public Action MessageRead;
        
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Text _title;
        [SerializeField] private Text _message;

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(CloseMessage);
            _nextButton.onClick.AddListener(CloseMessage);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(CloseMessage);
            _nextButton.onClick.RemoveListener(CloseMessage);
        }

        private void CloseMessage() => 
            base.Close(() => MessageRead?.Invoke());

        public void SetMessage(string title, string message)
        {
            _title.text = title;
            _message.text = message;
        }
    }
}