using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Main
{
    public class RecycleManager : MonoBehaviour
    {
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
            DoAnimation(trashObject, trashCan);
        }

        private void OnFail(TrashObject trashObject)
        {
            var trashCans = FindObjectsOfType<TrashCan>();
            var trashCan = trashCans.FirstOrDefault(
                c => c.TrashCanData.Type == trashObject.TrashData.Type);

            if (trashCan != null) 
                DoAnimation(trashObject, trashCan);
        }

        private static void DoAnimation(TrashObject trashObject, TrashCan trashCan)
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
            animationTween.OnComplete(() => trashObject.gameObject.SetActive(false));
        }
    }
}