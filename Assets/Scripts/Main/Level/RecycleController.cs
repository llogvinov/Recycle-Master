using System;
using DG.Tweening;
using Main;
using UnityEngine;

public static class RecycleController
{
    public static TrashCan TrashCan { get; set; }
    
    private static Vector3 _rotationBeforeDisposal;

    private const float AnimationDuration = 0.5f;
    private const float CenterYPosition = 4f;
    
    public static void DisposeAnimation(TrashObject trashObject, TrashCan trashCan, Action onComplete = default)
    {
        const float animationDurationPart = AnimationDuration / 2;
        const float midScaler = 1.2f;
        _rotationBeforeDisposal = new Vector3(90f, 0f, 0f);

        var animationTween = DOTween.Sequence();

        animationTween.Append(trashObject.transform.DOMove(trashCan.ObjectStartPoint.position, animationDurationPart).SetEase(Ease.InSine));
        animationTween.Insert(0f, trashObject.transform.DORotate(_rotationBeforeDisposal, animationDurationPart).SetEase(Ease.InSine));
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
        });
    }
}