using ElementGame.Data;
using ElementGame.Save;
using UnityEngine;
using Zenject;

namespace ElementGame.Core
{
    public class GameController : MonoBehaviour
    {
        private GameStateMachine _fsm;
        private GameContext _context;

        private BoardGenerator _boardGenerator;
        private LevelDatabase _levelDatabase;
        private SaveSystem _saveSystem;
        private DestroySystem _destroySystem;
        private GravityAnimator _gravityAnimator;

        [Inject]
        public void Construct(
            BoardGenerator boardGenerator,
            LevelDatabase levelDatabase,
            SaveSystem saveSystem,
            DestroySystem destroySystem,
            GravityAnimator gravityAnimator)
        {
            _boardGenerator = boardGenerator;
            _levelDatabase = levelDatabase;
            _saveSystem = saveSystem;
            _destroySystem = destroySystem;
            _gravityAnimator = gravityAnimator;
        }

        private void Awake()
        {
            _context = new GameContext
            {
                LevelDatabase = _levelDatabase,
                BoardGenerator = _boardGenerator,
                SaveSystem = _saveSystem,
                DestroySystem = _destroySystem,
                GravityAnimator = _gravityAnimator
            };

            _fsm = new GameStateMachine(this);
        }

        private void Start()
        {
            LoadLevelFromSaveOrDefault();
            _fsm.SetState(new IdleState(_context, _fsm));
        }

        public void TryPlayerMove(Vector2Int from, Vector2Int to)
        {
            if (_fsm.IsBusy) return;
            if (!_context.Board.IsInside(from) || !_context.Board.IsInside(to)) return;

            _fsm.SetState(new PlayerMoveState(_context, _fsm, from, to));
        }

        private void LoadLevelFromSaveOrDefault()
        {
            if (_saveSystem.HasSave())
            {
                SaveData save = _saveSystem.LoadGame();
                _context.CurrentLevelIndex = save.levelIndex;

                LevelConfig config = _levelDatabase.GetLevel(_context.CurrentLevelIndex);
                _context.Board = _boardGenerator.GenerateFromSave(save, config);
            }
            else
            {
                LevelConfig config = _levelDatabase.GetLevel(_context.CurrentLevelIndex);
                _context.Board = _boardGenerator.GenerateBoard(config);
            }
        }

        public void RestartLevel()
        {
            if (_fsm.IsBusy) return;
            LoadLevel(_context.CurrentLevelIndex);
        }

        public void LoadNextLevel()
        {
            if (_fsm.IsBusy) return;
            LoadLevel(_context.CurrentLevelIndex + 1);
        }

        private void LoadLevel(int levelIndex)
        {
            ClearBoardViews(); 

            _context.CurrentLevelIndex = levelIndex;

            LevelConfig config = _context.LevelDatabase.GetLevel(levelIndex);
            _context.Board = _context.BoardGenerator.GenerateBoard(config);

            _context.SaveSystem.SaveGame(levelIndex, _context.Board);

            _fsm.SetState(new IdleState(_context, _fsm));
        }

        private void ClearBoardViews()
        {
            if (_context.Board == null) return;

            for (int x = 0; x < _context.Board.Width; x++)
            {
                for (int y = 0; y < _context.Board.Height; y++)
                {
                    var cell = _context.Board.GetCell(new Vector2Int(x, y));
                    if (cell.Element?.View != null)
                    {
                        _context.DestroySystem.ReturnImmediate(cell.Element.View);
                        cell.Element.View = null;
                    }
                }
            }
        }

        private void OnApplicationQuit()
        {
            _saveSystem.SaveGame(_context.CurrentLevelIndex, _context.Board);
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
                _saveSystem.SaveGame(_context.CurrentLevelIndex, _context.Board);
        }
    }
}