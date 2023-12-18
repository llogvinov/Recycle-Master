using System;
using System.Collections;
using Core.Audio;
using DG.Tweening;
using UnityEngine;

namespace Main
{
    public class RecycleManager : MonoBehaviour
    {
        public static Action AllObjectsOfSpawnerThrown;

        [SerializeField] private AudioPlayer _throwAwayAudioPlayer;
        [SerializeField] private AudioPlayer _throwTrashAudioPlayer;
        private static Vector3 _rotationBeforeDisposal;

        private const float AnimationDuration = 0.5f;
        private const float CenterYPosition = 4f;

        private void Start()
        {
            TrashCanColliderChecker.Success += OnSuccess;
            TrashCanColliderChecker.Fail += OnFail;
        }

        private void OnDestroy()
        {
            TrashCanColliderChecker.Success -= OnSuccess;
            TrashCanColliderChecker.Fail -= OnFail;
        }

        private void OnSuccess(TrashObject trashObject, TrashCan trashCan)
        {
            DisposeAnimation(trashObject, trashCan, onComplete: () =>
            {
                _throwTrashAudioPlayer.Switch(play: true);
                trashCan.FXDust.Play();
            });
        }

        private void OnFail(TrashObject trashObject)
        {
            if (Timer.HasInstance)
                Timer.Instance.ReduceTime(5f); // todo: change this

            PushToCenter(trashObject,
                onStart: () => _throwAwayAudioPlayer.Switch(play: true),
                onComplete: () =>
                {
                    trashObject.IsThrown = false;
                    trashObject.ToggleInteraction(true);
                });
        }

        private static void DisposeAnimation(TrashObject trashObject, TrashCan trashCan, Action onComplete = default)
        {
            const float animationDurationPart = AnimationDuration / 2;
            const float midScaler = 0.8f;
            _rotationBeforeDisposal = new Vector3(90f, 0f, 0f);

            var animationTween = DOTween.Sequence();
            animationTween.Append(trashObject.transform.DORotate(_rotationBeforeDisposal, animationDurationPart).SetEase(Ease.InSine));
            animationTween.Insert(0f,
                trashObject.transform.DOMove(trashCan.ObjectStartPoint.position, animationDurationPart).SetEase(Ease.InSine));
            animationTween.Insert(0f,
                trashObject.transform.DOScale(Vector3.one * midScaler, animationDurationPart).SetEase(Ease.InSine));
            animationTween.Append(trashObject.transform.DOMove(trashCan.ObjectEndPoint.position, animationDurationPart).SetEase(Ease.OutSine));
            animationTween.Insert(animationDurationPart,
                trashObject.transform.DOScale(Vector3.zero, animationDurationPart).SetEase(Ease.OutSine));
            
            animationTween.OnComplete(() =>
            {
                onComplete?.Invoke();
                trashObject.gameObject.SetActive(false);
                trashObject.TrashObjectSpawner.ObjectThrown(trashObject);
                if (trashObject.TrashObjectSpawner.AllObjectsThrown)
                    AllObjectsOfSpawnerThrown?.Invoke();
            });
        }

        private void PushToCenter(TrashObject trashObject, Action onStart = default, Action onComplete = default)
        {
            trashObject.transform
                .DOMove(Vector3.up * CenterYPosition, AnimationDuration)
                .OnStart(() => onStart?.Invoke())
                .OnComplete(() => onComplete?.Invoke());
        }
    }
}