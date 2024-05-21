using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITimer : UIBase
    {
        [SerializeField] private Text _timerText;

        public void UpdateRemainingTime(float remainingTime)
        {
            var minutes = 0f;
            var seconds = 0f;
            
            if (remainingTime > 0f)
            {
                minutes = Mathf.FloorToInt(remainingTime / 60);
                seconds = Mathf.CeilToInt(remainingTime % 60);
            }
            _timerText.text = $"{minutes:0}:{seconds:00}";
        }
    }
}