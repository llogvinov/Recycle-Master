using UnityEngine;

namespace UI.Base
{
    public class UIBase : MonoBehaviour
    {
        [SerializeField] private Transform _content;

        protected Transform Content => _content;

        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
    }
}