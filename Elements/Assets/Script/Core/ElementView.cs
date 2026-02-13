using DG.Tweening;
using ElementGame.Pool;
using System;
using UnityEngine;

namespace ElementGame.Core
{
    public class ElementView : APoolableObject
    {
        [SerializeField] private float _moveSpeed = 0.15f;
        [SerializeField] private Animator _animator;

        private Tween _moveTween;
        private Action _destroyCallback;
        private Func<Vector2Int, Vector3> _gridToWorld;
        private Element _element;

        public Element Element => _element;

        public override void OnGet()
        {
            _animator.Rebind();
            float randomOffset = UnityEngine.Random.value;
            _animator.Play(0, 0, randomOffset);
            _animator.Update(0f);
        }

        public override void OnReturnToPool()
        {
            _moveTween?.Kill();
            _destroyCallback = null;
        }

        public void Initialize(Element element, Func<Vector2Int, Vector3> gridToWorld)
        {
            _element = element;
            _gridToWorld = gridToWorld;
            transform.position = _gridToWorld(element.Position);
        }

        public Vector3 GetWorldPosition(Vector2Int gridPos)
        {
            return _gridToWorld(gridPos);
        }

        public void PlayMove(Vector3 target, Action onComplete)
        {
            _moveTween?.Kill();

            _moveTween = transform.DOMove(target, _moveSpeed)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                });
        }

        public void PlayDestroy(Action onComplete)
        {
            _destroyCallback = onComplete;

            _animator.ResetTrigger("Death");
            _animator.SetTrigger("Death");
        }

        public void NotifyDeathFinished()
        {
            _destroyCallback?.Invoke();
            _destroyCallback = null;
        }
    }
}
