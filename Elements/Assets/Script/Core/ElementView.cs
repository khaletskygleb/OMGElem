using UnityEngine;
using System;
using DG.Tweening;
using ElementGame.Pool;

namespace ElementGame.Core
{
    public class ElementView : APoolableObject
    {
        [SerializeField] private float _animMoveSpeed = 0.15f;

        private Element _element;
        private Board _board;
        private Func<Vector2Int, Vector3> _gridToWorld;
        private Tween _moveTween;

        public Element Element => _element;

        public override void OnGet()
        {
            base.OnGet();
            transform.localScale = Vector3.one;
        }

        public override void OnReturnToPool()
        {
            transform.DOKill();
            StopAllCoroutines();

            if (_board != null)
            {
                _board = null;
            }
        }

        public void Initialize(Element element, Board board, Func<Vector2Int, Vector3> gridToWorld)
        {
            _element = element;
            _board = board;
            _gridToWorld = gridToWorld;

            transform.position = _gridToWorld(element.Position);
        }

        public void PlayMove(Vector3 target, Action onComplete)
        {
            _moveTween?.Kill();

            _moveTween = transform.DOMove(target, _animMoveSpeed)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => onComplete?.Invoke());
        }

        public void PlayDestroy(Action onComplete)
        {
            transform.DOScale(0f, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(() => onComplete?.Invoke());
        }

        public Vector3 GetWorldPosition(Vector2Int gridPos)
        {
            return _gridToWorld(gridPos);
        }
    }
}
