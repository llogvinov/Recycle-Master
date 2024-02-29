using System;
using UnityEngine;

namespace UI.Base
{
    public class UIBase : MonoBehaviour
    {
        [SerializeField] private Transform _content;

        protected Transform Content => _content;

        public virtual void Open(Action onComplete = default)
        {
            _content.gameObject.SetActive(true);
            onComplete?.Invoke();
        }

        public virtual void Close(Action onComplete = default)
        {
            _content.gameObject.SetActive(false);
            onComplete?.Invoke();
        }
    }
}