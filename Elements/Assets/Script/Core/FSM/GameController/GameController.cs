using ElementGame.Data;
using ElementGame.Save;
using UnityEngine;
using UnityEngine.SceneManagement;
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
                GravityAnimator = _gravityAnimator,
                CurrentLevelIndex = 0
            };

            _fsm = new GameStateMachine(this);
        }

        private void Start()
        {
            LoadLevelFromSaveOrDefault();
        }

        public void TryPlayerMove(Vector2Int from, Vector2Int to)
        {
            if (_context.IsResolving)
                return;

            if (!_context.Board.IsInside(from) || !_context.Board.IsInside(to))
                return;

            Element first = _context.Board.GetCell(from).Element;
            Element second = _context.Board.GetCell(to).Element;

            if (first == null)
                return;

            if (!first.IsInteractable)
                return;

            if (second != null && !second.IsInteractable)
                return;

            _fsm.SetState(new PlayerMoveState(_context, _fsm, from, to));
        }

        private void LoadLevelFromSaveOrDefault()
        {
            if (_saveSystem.HasSave())
            {
                SaveData save = _saveSystem.LoadGame();

                _context.CurrentLevelIndex = save.levelIndex;

                LevelConfig config =
                    _levelDatabase.GetLevel(_context.CurrentLevelIndex);

                if (save.cells != null &&
                    save.cells.Length == save.width * save.height)
                {
                    _context.Board =
                        _boardGenerator.GenerateFromSave(save, config);
                }
                else
                {
                    _context.Board =
                        _boardGenerator.GenerateBoard(config);
                }
            }
            else
            {
                LevelConfig config =
                    _levelDatabase.GetLevel(_context.CurrentLevelIndex);

                _context.Board =
                    _boardGenerator.GenerateBoard(config);
            }

            _fsm.SetState(new IdleState(_context, _fsm));
        }

        public void RestartLevel()
        {
            LoadLevel(_context.CurrentLevelIndex);
        }

        public void LoadNextLevel()
        {
            LoadLevel(_context.CurrentLevelIndex + 1);
            _saveSystem.SaveProgressOnly(_context.CurrentLevelIndex);
        }

        private void LoadLevel(int levelIndex)
        {
            _context.IsResolving = false;

            ClearCurrentBoard();

            _context.CurrentLevelIndex = levelIndex;

            LevelConfig config =
                _levelDatabase.GetLevel(levelIndex);

            _context.Board =
                _boardGenerator.GenerateBoard(config);

            _fsm = new GameStateMachine(this); 
            _fsm.SetState(new IdleState(_context, _fsm));
        }


        private void ClearCurrentBoard()
        {
            if (_context.Board == null)
                return;

            for (int x = 0; x < _context.Board.Width; x++)
            {
                for (int y = 0; y < _context.Board.Height; y++)
                {
                    var cell = _context.Board.GetCell(new Vector2Int(x, y));
                    if (cell.IsEmpty) continue;

                    var element = cell.Element;

                    if (element.View != null)
                    {
                        _destroySystem.ReturnImmediate(element.View);
                        element.View = null;
                    }
                }
            }

            _context.Board = null;
        }

        private void OnApplicationQuit()
        {
            SaveStable();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
                SaveStable();
        }

        private void SaveStable()
        {
            if (_context.Board == null)
                return;

            _saveSystem.SaveGame(
                _context.CurrentLevelIndex,
                _context.Board
            );
        }
    }
}
