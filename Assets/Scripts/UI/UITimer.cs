using DG.Tweening;
using Main;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITimer : UIBase
    {
        [SerializeField] private Text _timerText;
        [SerializeField] private Image _errorScreen;

        private void OnEnable()
        {
            TrashCanColliderChecker.Fail += FlashErrorScreen;
        }

        private void OnDisable()
        {
            TrashCanColliderChecker.Fail -= FlashErrorScreen;
        }

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

        private void FlashErrorScreen(TrashObject trashObject)
        {
            const float alphaValue = 0.3f;
            const float flashDuration = 0.15f;
            
            _errorScreen
                .DOFade(alphaValue, flashDuration)
                .SetLoops(2, LoopType.Yoyo);
        }
    }
}