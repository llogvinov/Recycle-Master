using System;
using System.Collections;
using Core;
using UI;
using UnityEngine;

namespace Main
{
    public class Timer
    {
        public Action OnStart;
        public Action OnFinish;

        private readonly ICoroutineRunner _coroutineRunner;
        private readonly UITimer _uiTimer;
        
        private Coroutine _countdownCoroutine;
        private bool _pause;

        private static Timer _instance = null;
        
        public static Timer Instance
        {
            get 
            {
                if (_instance is null)
                    throw new ArgumentNullException($"Singleton is not yet created");

                return _instance;
            }
        }

        public static void Initialize(ICoroutineRunner coroutineRunner, UITimer uiTimer)
        {
            if (_instance is not null)
                throw new InvalidOperationException($"Singleton already created exists");
            
            _instance = new Timer(coroutineRunner, uiTimer);
        }

        private Timer(ICoroutineRunner coroutineRunner, UITimer uiTimer)
        {
            _coroutineRunner = coroutineRunner;
            _uiTimer = uiTimer;
        }

        public void StartCountdown(float seconds) =>
            _countdownCoroutine = _coroutineRunner.StartCoroutine(Countdown(seconds));

        public void StopCountdown()
        {
            if (_countdownCoroutine is not null) 
                _coroutineRunner.StopCoroutine(_countdownCoroutine);
        }

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

        public void PauseTimer() => _pause = true;
        public void ContinueTimer() => _pause = false;
    }
}