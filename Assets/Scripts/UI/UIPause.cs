using System;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPause : UIPanel
    {
        public static Action PauseButtonClicked;
        public static Action LeaveButtonClicked;
        public static Action ContinueButtonClicked;
        
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _leaveButton;
        [SerializeField] private Button _continueButton;

        private void Start()
        {
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
            _leaveButton.onClick.AddListener(OnLeaveButtonClicked);
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        private void OnDestroy()
        {
            _pauseButton.onClick.RemoveListener(OnPauseButtonClicked);
            _leaveButton.onClick.RemoveListener(OnLeaveButtonClicked);
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        }

        private void OnPauseButtonClicked()
        {
            base.Open();
            _pauseButton.gameObject.SetActive(false);
            PauseButtonClicked?.Invoke();
        }

        private void OnLeaveButtonClicked()
        {
            base.Close(onComplete:() =>
            {
                _pauseButton.gameObject.SetActive(true);
                LeaveButtonClicked?.Invoke();
            });
        }

        private void OnContinueButtonClicked()
        {
            base.Close(onComplete:() =>
            {
                _pauseButton.gameObject.SetActive(true);
                ContinueButtonClicked?.Invoke();
            });
        }
    }
}