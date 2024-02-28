using System;
using System.Collections;
using UnityEngine;

namespace Core.Tutorial
{
    public class CustomActionPart: ITutorialPart
    {
        private readonly Action _action;
        
        public CustomActionPart(Action action)
        {
            _action = action;
        }

        public event Action OnExecuted;
        
        public IEnumerator WaitForAction()
        {
            Debug.Log("custom part");
            _action?.Invoke();
            OnExecuted?.Invoke();
            yield break;
        }
    }
}