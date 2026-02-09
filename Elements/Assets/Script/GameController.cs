using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [SerializeField] private BoardGenerator _boardGenerator;

    private Board _board;
    private bool _isResolvingBoard;

    private int _activeTweens;

    private void OnEnable()
    {
        ElementView.TweenStarted += OnTweenStarted;
        ElementView.TweenCompleted += OnTweenCompleted;
    }

    private void OnDisable()
    {
        ElementView.TweenStarted -= OnTweenStarted;
        ElementView.TweenCompleted -= OnTweenCompleted;
    }

    private void OnTweenStarted() => _activeTweens++;
    private void OnTweenCompleted() => _activeTweens--;

    private void Start()
    {
        _board = _boardGenerator.GenerateBoard();
    }

    public void TryPlayerMove(Vector2Int from, Vector2Int to)
    {
        if (_isResolvingBoard) return;
        if (!_board.IsInside(from) || !_board.IsInside(to)) return;

        StartCoroutine(PlayerMoveRoutine(from, to));
    }

    private IEnumerator PlayerMoveRoutine(Vector2Int from, Vector2Int to)
    {
        _isResolvingBoard = true;

        Cell fromCell = _board.GetCell(from);
        Cell toCell = _board.GetCell(to);

        Vector2Int dir = to - from;

        if (dir == Vector2Int.up && toCell.IsEmpty)
        {
            _isResolvingBoard = false;
            yield break;
        }

        _board.SwapElements(from, to);
        yield return WaitForBoardAnimation();

        DropFloating(to);
        yield return WaitForBoardAnimation();

        while (GravitySystem.Apply(_board))
            yield return WaitForBoardAnimation();

        yield return ResolveBoardRoutine();

        _isResolvingBoard = false;
    }

    private void DropFloating(Vector2Int startPos)
    {
        Vector2Int current = startPos;

        while (true)
        {
            Vector2Int below = current + Vector2Int.down;

            if (!_board.IsInside(below)) break;
            if (!_board.GetCell(below).IsEmpty) break;

            _board.MoveElement(current, below);
            current = below;
        }
    }

    private IEnumerator ResolveBoardRoutine()
    {
        while (true)
        {
            List<Cell> matches = MatchFinder.FindAllMatches(_board);
            if (matches.Count == 0)
                yield break;

            yield return DestroySystem.RemoveWithAnimation(_board, matches);

            while (GravitySystem.Apply(_board))
                yield return WaitForBoardAnimation();
        }
    }

    private IEnumerator WaitForBoardAnimation()
    {
        while (_activeTweens > 0)
            yield return null;
    }
}