using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Tutorial
{
    public class TriggerPart : ITutorialPart
    {
        private UnityEvent _trigger;
        public event Action OnExecuted;

        public TriggerPart(UnityEvent trigger)
        {
            _trigger = trigger;
        }
        
        public IEnumerator WaitForAction()
        {
            Debug.Log("trigger part");
            var triggered = false;

            _trigger.AddListener(() => triggered= true);

            while (!triggered)
            {
                yield return null;
            }
            
            OnExecuted?.Invoke();
        }
    }
}