using System;
using UnityEngine;

namespace UI.Base
{
    public class UIBase : MonoBehaviour
    {
        [SerializeField] private Transform _content;

        protected Transform Content => _content;

        public virtual void Open()
        {
            _content.gameObject.SetActive(true);
        }

        public virtual void Close(Action onComplete = default)
        {
            _content.gameObject.SetActive(false);
            onComplete?.Invoke();
        }
    }
}