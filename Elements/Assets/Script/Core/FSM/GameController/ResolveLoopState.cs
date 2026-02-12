using System.Collections;

namespace ElementGame.Core
{
    public class ResolveLoopState : IGameState
    {
        private GameContext _context;
        private GameStateMachine _fsm;

        public ResolveLoopState(GameContext context, GameStateMachine fsm)
        {
            _context = context;
            _fsm = fsm;
        }

        public IEnumerator Enter()
        {
            while (true)
            {
                var matches = MatchFinder.FindAllMatches(_context.Board);
                if (matches.Count == 0)
                    break;

                yield return _fsm.RunSubState(new DestroyState(_context, matches));
                yield return _fsm.RunSubState(new GravityState(_context));
            }

            if (!_context.Board.HasAnyElements())
                _fsm.SetState(new LevelCompleteState(_context, _fsm));
        }
    }
}