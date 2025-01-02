using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;

namespace _Content.Scripts.Utilities
{
    public static class DOTweenExtension
    {
        private const string ScaleIdKEy = "ScaleTween";
        private const string MoveIdKEy = "MoveTween";
        
        public static TweenerCore<Vector3, Vector3, VectorOptions> ScaleExtension(this Transform t, Vector3 scale,
            float duration, Ease ease = Ease.OutBack, UnityAction onCompleted = null)
        {
            DOTween.Complete(ScaleIdKEy + t.GetInstanceID());
            return t.DOScale(scale, duration).SetEase(ease).SetLink(t.gameObject)
                .OnComplete(() => onCompleted?.Invoke()).SetId(ScaleIdKEy + t.GetInstanceID());
        }
        
        public static TweenerCore<Vector3, Vector3, VectorOptions> MoveExtension(this Transform t, Vector3 target,
            float duration, Ease ease = Ease.Linear, UnityAction onCompleted = null)
        {
            DOTween.Complete(MoveIdKEy + t.GetInstanceID());
            return t.DOMove(target, duration).SetEase(ease).SetLink(t.gameObject)
                .OnComplete(() => onCompleted?.Invoke()).SetId(MoveIdKEy + t.GetInstanceID());
        }
    }
}