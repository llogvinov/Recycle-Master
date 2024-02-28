using System;
using DG.Tweening;
using ObjectsData;
using UnityEngine;
using UnityEngine.Events;

namespace Main
{
    public class TrashObject : MonoBehaviour
    {
        public UnityEvent OnDisposed;
        
        [SerializeField] private Vector3 _dragRotation;

        public TrashObjectSpawner TrashObjectSpawner { get; private set; }
        public TrashData TrashData { get; private set; }
        public bool IsInteractable { get; set; }
        public bool IsDisposed { get; set; }

        private Tween _rotateTween;

        public void Init(TrashData trashData, TrashObjectSpawner trashObjectSpawner)
        {
            TrashObjectSpawner = trashObjectSpawner;
            TrashData = trashData;
            IsDisposed = false;
            IsInteractable = true;
        }

        public void ToggleInteraction(bool enable) =>
            IsInteractable = enable;
    }
}