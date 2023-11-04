﻿using DG.Tweening;
using UnityEngine;

namespace Main
{
    public class RecycleManager : MonoBehaviour
    {
        private const float AnimationDuration = 0.5f;
        
        private void Start()
        {
            ColliderChecker.Success += OnSuccess;
            ColliderChecker.Fail += OnFail;
        }

        private void OnDestroy()
        {
            ColliderChecker.Success -= OnSuccess;
            ColliderChecker.Fail -= OnFail;
        }

        private void OnSuccess(TrashObject trashObject, TrashCan trashCan)
        {
            var animationDurationPart = AnimationDuration / 2;
            
            var animationTween = DOTween.Sequence();
            animationTween.Append(trashObject.transform.DOMove(trashCan.ObjectStartPoint.position, animationDurationPart));
            animationTween.Append(trashObject.transform.DOMove(trashCan.ObjectEndPoint.position, animationDurationPart));
            animationTween.Insert(animationDurationPart, trashObject.transform.DOScale(Vector3.zero, animationDurationPart));
            animationTween.OnComplete(() => trashObject.gameObject.SetActive(false));
        }

        private void OnFail(TrashObject obj)
        {
            
        }
    }
}