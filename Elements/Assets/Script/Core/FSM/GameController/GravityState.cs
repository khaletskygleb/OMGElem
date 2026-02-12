using System.Collections;

namespace ElementGame.Core
{
    public class GravityState : IGameState
    {
        private GameContext _context;

        public GravityState(GameContext context)
        {
            _context = context;
        }

        public IEnumerator Enter()
        {
            while (true)
            {
                var falls = GravitySystem.CalculateFalls(_context.Board);
                if (falls.Count == 0) break;

                yield return _context.GravityAnimator.Play(_context.Board, falls, _context.BoardGenerator.GridToWorld);
            }
        }
    }
}