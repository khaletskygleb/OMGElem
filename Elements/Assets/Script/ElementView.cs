using UnityEngine;
using System;
using DG.Tweening;

public class ElementView : MonoBehaviour
{
    private Element _element;
    private Board _board;
    private Func<Vector2Int, Vector3> _gridToWorld;
    private Tween _moveTween;

    public Action<ElementView> OnDestroyAnimationFinished;
    public static event Action TweenStarted;
    public static event Action TweenCompleted;

    public void PlayDestroyAnimation()
    {
        transform.DOScale(0f, 0.2f).OnComplete(() =>
        {
            OnDestroyAnimationFinished?.Invoke(this);
        }).SetDelay(0.5f);
    }

    public Element Element => _element;

    public void Initialize(Element element, Board board, Func<Vector2Int, Vector3> gridToWorldFunc)
    {
        _element = element;
        _board = board;
        _gridToWorld = gridToWorldFunc;

        transform.position = _gridToWorld(element.Position);
        board.OnElementMoved += HandleMoved;
        board.OnElementRemoved += HandleElementRemoved;
    }

    private void HandleMoved(Element movedElement, Vector2Int from, Vector2Int to)
    {
        if (movedElement != _element)
            return;

        _moveTween?.Kill();

        Vector3 sideStep = _gridToWorld(new Vector2Int(to.x, from.y));
        Vector3 finalPos = _gridToWorld(to);

        void FinishTween()
        {
            TweenCompleted?.Invoke();
        }

        TweenStarted?.Invoke();

        if (from.x != to.x && from.y != to.y) 
        {
            _moveTween = transform.DOMove(sideStep, 0.1f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    TweenStarted?.Invoke();

                    _moveTween = transform.DOMove(finalPos, 0.12f)
                        .SetEase(Ease.InQuad)
                        .OnComplete(FinishTween);
                });
        }
        else
        {
            _moveTween = transform.DOMove(finalPos, 0.15f)
                .SetEase(Ease.OutQuad)
                .OnComplete(FinishTween);
        }
    }

    private void HandleElementRemoved(Element removedElement)
    {
        if (removedElement != _element) return;

        _moveTween?.Kill();

        transform.DOScale(0f, 0.15f)
            .SetEase(Ease.InBack)
            .OnComplete(() => Destroy(gameObject));
    }

    private void OnDestroy()
    {
        if (_board == null) return;

        _board.OnElementMoved -= HandleMoved;
        _board.OnElementRemoved -= HandleElementRemoved;
    }
}
