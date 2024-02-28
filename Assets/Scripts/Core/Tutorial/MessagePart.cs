using System;
using System.Collections;
using UI;
using UnityEngine;

namespace Core.Tutorial
{
    public class MessagePart : ITutorialPart
    {
        public event Action OnExecuted;
        
        private readonly UIMessage _uiMessage;
        private readonly string _message;
        private readonly Action _onClick;

        public MessagePart(UIMessage uiMessage, string message, Action onClick = default)
        {
            _uiMessage = uiMessage;
            _message = message;
            _onClick = onClick;
        }

        public IEnumerator WaitForAction()
        {
            var clicked = false;
            _uiMessage.SetMessage(_message);
            _uiMessage.Open(() =>
            {
                _uiMessage.SkipButton.onClick.AddListener(() =>
                {
                    _uiMessage.Close(() =>
                    {
                        clicked = true;
                        _onClick?.Invoke();
                        OnExecuted?.Invoke();
                    });
                });
            });
            
            while (!clicked)
            {
                yield return null;
            }
        }
    }
}