using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Tutorial.UI
{
    public class TutorialMessages : MonoBehaviour
    {
        [SerializeField] private List<Message> _messages;

        private int _currentMessage;

        public List<Message> Messages => _messages;

        public void Init()
        {
            _currentMessage = -1;
            EnableNext();
        }
        
        public void DisableAll()
        {
            foreach (var message in _messages) 
                message.gameObject.SetActive(false);
        }

        public void SwitchToNext()
        {
            DisableCurrent();
            EnableNext();
        }

        public void DisableCurrent() => 
            _messages[_currentMessage].gameObject.SetActive(false);

        public void EnableNext()
        {
            _currentMessage++;
            if (_currentMessage > _messages.Count)
                throw new ArgumentOutOfRangeException(nameof(_messages), $"{nameof(_currentMessage)} is out of range");

            _messages[_currentMessage].gameObject.SetActive(true);
        }
    }
}