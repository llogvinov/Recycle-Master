using DG.Tweening;
using UnityEngine;

namespace UI.Base
{
    public class UIPanel : UIBase
    {
        public const float AnimationDuration = 0.5f;

        public override void Open()
        {
            Content.localScale = Vector3.zero;
            base.Open();
            Content.DOScale(Vector3.one, AnimationDuration);
        }

        public override void Close()
        {
            Content.DOScale(Vector3.zero, AnimationDuration)
                .OnComplete(base.Close);
        }
    }
}