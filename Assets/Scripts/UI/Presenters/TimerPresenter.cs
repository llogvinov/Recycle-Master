using System.Collections;
using UI.Views;
using UnityEngine;

namespace UI.Presenters
{
    public class TimerPresenter : BasePresenter
    {
        [SerializeField] private TimerScreenView _view;
    
        private Coroutine _countdownCoroutine;

        public TimerScreenView View => _view;

        public void StartCountdown(float seconds) =>
            _countdownCoroutine = StartCoroutine(Countdown(seconds));

        private IEnumerator Countdown(float seconds)
        {
            var counter = seconds;
            while (counter > 0)
            {
                yield return null;
                counter -= Time.deltaTime;
                UpdateRemainingTime(counter);
            }

            gameObject.SetActive(false);
        }

        private void UpdateRemainingTime(float remainingTime)
        { 
            float minutes = Mathf.FloorToInt(remainingTime / 60);  
            float seconds = Mathf.FloorToInt(remainingTime % 60);
            _view.TimerText.text = $"{minutes:0}:{seconds:00}";
        }

        public void StopCountdown()
        {
            if (_countdownCoroutine != null) 
                StopCoroutine(_countdownCoroutine);
        }
    
    }
}
