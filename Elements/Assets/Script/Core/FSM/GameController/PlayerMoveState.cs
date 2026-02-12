using System.Collections;
using UnityEngine;

namespace ElementGame.Core
{
    public class PlayerMoveState : IGameState
    {
        private GameContext _context;
        private GameStateMachine _fsm;
        private Vector2Int _from, _to;

        public PlayerMoveState(GameContext context, GameStateMachine fsm, Vector2Int from, Vector2Int to)
        {
            _context = context;
            _fsm = fsm;
            _from = from;
            _to = to;
        }

        public IEnumerator Enter()
        {
            if (!_context.Board.IsInside(_to))
                yield break;

            Cell toCell = _context.Board.GetCell(_to);

            Vector2Int dir = _to - _from;

            if (dir == Vector2Int.up && toCell.IsEmpty)
                yield break;

            if (toCell.IsEmpty)
                yield return MoveToEmpty();
            else
                yield return Swap();

            yield return _fsm.RunSubState(new GravityState(_context));
            yield return _fsm.RunSubState(new ResolveLoopState(_context, _fsm));
        }

        private IEnumerator MoveToEmpty()
        {
            Element element = _context.Board.GetCell(_from).Element;
            if (element == null)
                yield break;

            element.State = ElementState.Moving;

            bool done = false;
            var tracker = new AnimationTracker();
            tracker.Setup(() => done = true);

            tracker.Register();
            element.View.PlayMove(
                element.View.GetWorldPosition(_to),
                tracker.Complete);

            yield return new WaitUntil(() => done);

            _context.Board.MoveElement(_from, _to);

            yield return FallElementIfNeeded(element);

            element.State = ElementState.Idle;
        }

        private IEnumerator FallElementIfNeeded(Element element)
        {
            Vector2Int startPos = element.Position;
            Vector2Int finalPos = startPos;

            while (true)
            {
                Vector2Int below = finalPos + Vector2Int.down;

                if (!_context.Board.IsInside(below))
                    break;

                if (!_context.Board.GetCell(below).IsEmpty)
                    break;

                finalPos = below;
            }

            if (finalPos == startPos)
                yield break;

            element.State = ElementState.Falling;

            bool done = false;
            var tracker = new AnimationTracker();
            tracker.Setup(() => done = true);

            tracker.Register();
            element.View.PlayMove(
                element.View.GetWorldPosition(finalPos),
                tracker.Complete);

            yield return new WaitUntil(() => done);

            _context.Board.MoveElement(startPos, finalPos);
        }

        private IEnumerator Swap()
        {
            Element first = _context.Board.GetCell(_from).Element;
            Element second = _context.Board.GetCell(_to).Element;

            if (first == null || second == null)
                yield break;

            first.State = ElementState.Moving;
            second.State = ElementState.Moving;

            bool done = false;
            var tracker = new AnimationTracker();
            tracker.Setup(() => done = true);

            tracker.Register();
            first.View.PlayMove(
                first.View.GetWorldPosition(_to),
                tracker.Complete);

            tracker.Register();
            second.View.PlayMove(
                second.View.GetWorldPosition(_from),
                tracker.Complete);

            yield return new WaitUntil(() => done);

            _context.Board.SwapElements(_from, _to);

            first.State = ElementState.Idle;
            second.State = ElementState.Idle;
        }
    }
}
