using System;
using DG.Tweening;
using Main;
using UnityEngine;

public static class RecycleController
{
    public static Action AllObjectsOfSpawnerThrown;
    
    public static TrashCan TrashCan { get; set; }
    
    private static Vector3 _rotationBeforeDisposal;

    private const float AnimationDuration = 0.5f;
    
    public static void DisposeAnimation(TrashObject trashObject, TrashCan trashCan, Action onComplete = default)
    {
        const float midScaler = 1.2f;
        _rotationBeforeDisposal = new Vector3(90f, 0f, 0f);

        var animationTween = DOTween.Sequence();

        animationTween.Append(trashObject.transform.DOMove(trashCan.ObjectStartPoint.position, AnimationDuration).SetEase(Ease.InSine));
        animationTween.Insert(0f, trashObject.transform.DORotate(_rotationBeforeDisposal, AnimationDuration).SetEase(Ease.InSine));
        animationTween.Insert(0f,
            trashObject.transform.DOScale(Vector3.one * midScaler, AnimationDuration).SetEase(Ease.InSine));
        animationTween.Append(trashObject.transform.DOMove(trashCan.ObjectEndPoint.position, AnimationDuration/2).SetEase(Ease.OutSine));
        animationTween.Insert(AnimationDuration,
            trashObject.transform.DOScale(Vector3.zero, AnimationDuration/2).SetEase(Ease.OutSine));
            
        animationTween.OnComplete(() =>
        {
            onComplete?.Invoke();
            trashObject.gameObject.SetActive(false);
            trashObject.TrashObjectSpawner.ObjectThrown(trashObject);
            if (trashObject.TrashObjectSpawner.AllObjectsThrown)
                AllObjectsOfSpawnerThrown?.Invoke();
        });
    }
}