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
            Vector2Int dir = _to - _from;
            Cell toCell = _context.Board.GetCell(_to);

            if (dir == Vector2Int.up && toCell.IsEmpty)
                yield break;

            if (toCell.IsEmpty)
                yield return MoveToEmpty();
            else
                yield return Swap();

            yield return _fsm.RunSubState(new GravityState(_context));

            yield return _fsm.RunSubState(new ResolveLoopState(_context, _fsm));
            _fsm.SetState(new IdleState(_context, _fsm));
        }

        private IEnumerator MoveToEmpty()
        {
            Element element = _context.Board.GetCell(_from).Element;
            _context.Board.MoveElement(_from, _to);

            bool done = false;
            var tracker = new AnimationTracker();
            tracker.Setup(() => done = true);

            tracker.Register();
            element.View.PlayMove(element.View.GetWorldPosition(_to), tracker.Complete);

            yield return new WaitUntil(() => done);
        }

        private IEnumerator Swap()
        {
            Element firstElement = _context.Board.GetCell(_from).Element;
            Element secondElement = _context.Board.GetCell(_to).Element;

            bool done = false;
            var tracker = new AnimationTracker();
            tracker.Setup(() => done = true);

            if (firstElement != null)
            {
                tracker.Register();
                firstElement.View.PlayMove(firstElement.View.GetWorldPosition(_to), tracker.Complete);
            }

            if (secondElement != null)
            {
                tracker.Register();
                secondElement.View.PlayMove(secondElement.View.GetWorldPosition(_from), tracker.Complete);
            }

            yield return new WaitUntil(() => done);
            _context.Board.SwapElements(_from, _to);
        }
    }
}