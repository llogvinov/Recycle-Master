using System;
using System.Collections;
using Core;
using UnityEngine;

namespace UI
{
    public class Timer : MonoSingleton<Timer>
    {
        public Action OnStart;
        public Action OnFinish;
        
        private UITimer _uiTimer;
        private Coroutine _countdownCoroutine;
        private bool _pause;

        private void Awake() => 
            _uiTimer = GetComponent<UITimer>();

        public void StartCountdown(float seconds) =>
            _countdownCoroutine = StartCoroutine(Countdown(seconds));

        private IEnumerator Countdown(float seconds)
        {
            OnStart?.Invoke();
            
            var counter = seconds;
            while (counter > 0)
            {
                yield return null;
                if (_pause) continue;
                
                counter -= Time.deltaTime;
                _uiTimer.UpdateRemainingTime(counter);
            }

            OnFinish?.Invoke();
        }
        
        public void StopCountdown()
        {
            if (_countdownCoroutine != null) 
                StopCoroutine(_countdownCoroutine);
        }

        public void PauseTimer() => _pause = true;
        public void ContinueTimer() => _pause = false;
    }
}