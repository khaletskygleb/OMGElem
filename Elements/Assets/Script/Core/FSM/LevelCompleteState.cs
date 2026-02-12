using System.Collections;
using UnityEngine;

namespace ElementGame.Core
{
    public class LevelCompleteState : IGameState
    {
        private GameContext _context;
        private GameStateMachine _fsm;

        public LevelCompleteState(GameContext context, GameStateMachine fsm)
        {
            _context = context;
            _fsm = fsm;
        }

        public IEnumerator Enter()
        {
            yield return new WaitForSeconds(0.5f);

            _context.CurrentLevelIndex++;

            var config = _context.LevelDatabase.GetLevel(_context.CurrentLevelIndex);
            _context.Board = _context.BoardGenerator.GenerateBoard(config);

            _context.SaveSystem.SaveGame(_context.CurrentLevelIndex, _context.Board);

            _fsm.SetState(new IdleState(_context, _fsm));
        }
    }
}