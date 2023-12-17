using System;
using System.Linq;
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

        private const float AnimationDuration = 0.5f;

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
            DoAnimation(trashObject, trashCan, () =>
            {
                _throwTrashAudioPlayer.Switch(play: true);
                trashCan.FXDust.Play();
            });
        }

        private void OnFail(TrashObject trashObject)
        {
            var trashCans = FindObjectsOfType<TrashCan>();
            var trashCan = trashCans.FirstOrDefault(
                c => c.TrashCanData.Type == trashObject.TrashData.Type);
            if (trashCan == null) return;
            
            if (Timer.HasInstance)
                Timer.Instance.ReduceTime(5f); // todo: change this
            
            DoAnimation(trashObject, trashCan);
            _throwAwayAudioPlayer.Switch(play: true);
        }

        private static void DoAnimation(TrashObject trashObject, TrashCan trashCan, Action onFinish = default)
        {
            const float animationDurationPart = AnimationDuration / 2;
            const float midScaler = 0.5f;

            var animationTween = DOTween.Sequence();
            animationTween.Append(trashObject.transform.DOMove(trashCan.ObjectStartPoint.position, animationDurationPart));
            animationTween.Insert(animationDurationPart,
                trashObject.transform.DOScale(Vector3.one * midScaler, animationDurationPart).SetEase(Ease.OutQuint));
            animationTween.Append(trashObject.transform.DOMove(trashCan.ObjectEndPoint.position, animationDurationPart));
            animationTween.Insert(animationDurationPart,
                trashObject.transform.DOScale(Vector3.zero, animationDurationPart));
            animationTween.OnComplete(() =>
            {
                onFinish?.Invoke();
                trashObject.gameObject.SetActive(false);
                trashObject.TrashObjectSpawner.ObjectThrown(trashObject);
                if (trashObject.TrashObjectSpawner.AllObjectsThrown)
                    AllObjectsOfSpawnerThrown?.Invoke();
            });
        }
    }
}