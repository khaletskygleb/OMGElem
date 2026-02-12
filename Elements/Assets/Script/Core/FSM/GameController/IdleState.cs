using System.Collections;

namespace ElementGame.Core
{
    public class IdleState : IGameState
    {
        private GameContext _context;
        private GameStateMachine _fsm;

        public IdleState(GameContext context, GameStateMachine fsm)
        {
            _context = context;
            _fsm = fsm;
        }

        public IEnumerator Enter()
        {
            yield break;
        }
    }
}