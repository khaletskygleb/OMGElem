using ElementGame.Data;
using ElementGame.Pool;
using UnityEngine;
using Zenject;

namespace ElementGame.Core
{
    public class BoardGenerator : MonoBehaviour
    {
        [SerializeField] private AutoScaleBackground _scaleBackground;

        [Header("World Grid Settings")]
        [SerializeField] private Transform _boardOrigin;
        [SerializeField] private float _cellSize = 2f;

        private LevelConfig _levelConfig;
        private ObjectPoolService _pool;

        [Inject]
        public void Construct(ObjectPoolService pool)
        {
            _pool = pool;
        }

        public Board GenerateBoard(LevelConfig config)
        {
            return BuildBoard(
                config.Width,
                config.Height,
                config.Cells,
                config
            );
        }

        public Board GenerateFromSave(Save.SaveData save, LevelConfig config)
        {
            return BuildBoard(
                save.width,
                save.height,
                save.cells,
                config
            );
        }

        private Board BuildBoard(int width, int height, int[] cellTypeIndexes, LevelConfig config)
        {
            _levelConfig = config;

            Camera.main.orthographicSize =
                (_levelConfig.Width * _cellSize)  + 2f;

            _scaleBackground.ScaleToCamera();

            Board board = new Board(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int index = y * width + x;

                    if (index < 0 || index >= cellTypeIndexes.Length)
                        continue;

                    int typeIndex = cellTypeIndexes[index];

                    if (typeIndex < 0)
                        continue;

                    Vector2Int pos = new Vector2Int(x, y);

                    Element element = new Element(typeIndex, pos);

                    board.SpawnElement(pos, element);
                    SpawnView(element, board);
                }
            }

            return board;
        }

        private void SpawnView(Element element, Board board)
        {
            var definition =
                _levelConfig.Database.GetByIndex(element.TypeIndex);

            ElementView view = _pool.Get(definition.Prefab);

            view.Initialize(element, GridToWorld);

            element.View = view;
        }

        public Vector3 GridToWorld(Vector2Int gridPos)
        {
            float boardWidthWorld = _levelConfig.Width * _cellSize;
            float boardHeightWorld = _levelConfig.Height * _cellSize;

            float left = _boardOrigin.position.x - boardWidthWorld / 2f;
            float bottom = _boardOrigin.position.y;

            return new Vector3(
                left + gridPos.x * _cellSize + _cellSize / 2f,
                bottom + gridPos.y * _cellSize + _cellSize / 2f,
                0f
            );
        }
    }
}
