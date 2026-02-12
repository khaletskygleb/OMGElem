using System.Collections;
using System.Collections.Generic;

namespace ElementGame.Core
{
    public class DestroyState : IGameState
    {
        private GameContext _context;
        private List<Cell> _cells;

        public DestroyState(GameContext context, List<Cell> cells)
        {
            _context = context;
            _cells = cells;
        }

        public IEnumerator Enter()
        {
            foreach (var cell in _cells)
            {
                if (!cell.IsEmpty)
                    cell.Element.State = ElementState.Destroying;
            }

            yield return _context.DestroySystem.RemoveWithAnimation(_context.Board, _cells);
        }
    }
}