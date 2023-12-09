using System;
using System.Collections;
using Core;
using TMPro;
using UI;
using UnityEngine;

namespace Main
{
    public class Timer
    {
        public Action OnStart;
        public Action OnFinish;

        private const float MinCounter = 0f;
        private const float MaxCounter = 100000f;
        
        private ICoroutineRunner _coroutineRunner;
        private UITimer _uiTimer;

        private float _counter;
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

        public static bool HasInstance => _instance is not null;

        public static void Initialize(ICoroutineRunner coroutineRunner, UITimer uiTimer)
        {
            if (_instance is not null)
                throw new InvalidOperationException($"Singleton already created exists");
            
            _instance = new Timer(coroutineRunner, uiTimer);
        }

        private Timer(ICoroutineRunner coroutineRunner, UITimer uiTimer)
        {
            UpdateReferences(coroutineRunner, uiTimer);
        }

        public void UpdateReferences(ICoroutineRunner coroutineRunner, UITimer uiTimer)
        {
            _coroutineRunner = coroutineRunner;
            _uiTimer = uiTimer;
        }

        public void StartCountdown(float seconds)
        {
            _countdownCoroutine = _coroutineRunner.StartCoroutine(Countdown(seconds));
            
            IEnumerator Countdown(float seconds)
            {
                OnStart?.Invoke();
            
                _counter = seconds;
                while (_counter > 0)
                {
                    yield return null;
                    if (_pause) continue;
                
                    _counter -= Time.deltaTime;
                    _uiTimer.UpdateRemainingTime(_counter);
                }

                OnFinish?.Invoke();
            }
        }

        public void StopCountdown()
        {
            if (_countdownCoroutine is not null) 
                _coroutineRunner.StopCoroutine(_countdownCoroutine);
        }

        public void ReduceTime(float value) => 
            _counter = Mathf.Clamp(_counter - value, MinCounter, MaxCounter);
        
        public void AddTime(float value) => 
            _counter = Mathf.Clamp(_counter + value, MinCounter, MaxCounter);

        public void PauseTimer() => _pause = true;
        
        public void ContinueTimer() => _pause = false;
    }
}