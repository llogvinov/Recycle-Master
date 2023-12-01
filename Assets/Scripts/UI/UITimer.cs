using System.Collections;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITimer : UIBase
    {
        [SerializeField] private Text _timerText;
        
        private Coroutine _countdownCoroutine;

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
            _timerText.text = $"{minutes:0}:{seconds:00}";
        }

        public void StopCountdown()
        {
            if (_countdownCoroutine != null) 
                StopCoroutine(_countdownCoroutine);
        }
    }
}