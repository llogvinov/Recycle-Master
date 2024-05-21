using System;
using DG.Tweening;
using Main;
using Main.Level;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [Serializable]
    public class UIFlashScreenSettings
    {
        public float AlphaValue = 0.3f;
        public float FlashDuration = 0.15f;
        public int Loops = 2;
    }

    public class UIFlashScreen : MonoBehaviour
    {
        [SerializeField] private UIFlashScreenSettings _settings;
        [Space]
        [SerializeField] private Image _errorScreen;

        private Tween _currentFlash;
        
        private void OnEnable() => 
            ObjectSelectHandler.OnWrongSelected += FlashErrorScreen;

        private void OnDisable() => 
            ObjectSelectHandler.OnWrongSelected -= FlashErrorScreen;
        
        private void FlashErrorScreen(TrashObject trashObject)
        {
            if (_currentFlash.IsActive())
            {
                _currentFlash.Kill();
                ResetImageAlpha();
            }
            
            _currentFlash = _errorScreen
                .DOFade(_settings.AlphaValue, _settings.FlashDuration)
                .SetLoops(_settings.Loops, LoopType.Yoyo);

             void ResetImageAlpha()
             {
                 _errorScreen.color = new Color(_errorScreen.color.r, _errorScreen.color.g, _errorScreen.color.b, 0f);
             }
        }
    }
}