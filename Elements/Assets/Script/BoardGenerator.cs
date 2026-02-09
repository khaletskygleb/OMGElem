using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private AutoScaleBackground _scaleBackground;
    [SerializeField] private LevelConfig _levelConfig;

    [Header("World Grid Settings")]
    [SerializeField] private Transform _boardOrigin;
    [SerializeField] private float _cellSize = 2f; 

    public Board GenerateBoard()
    {
        Camera.main.orthographicSize = _levelConfig.Height * _cellSize + 1f;
        _scaleBackground.ScaleToCamera();   

        Board board = new Board(_levelConfig.Width, _levelConfig.Height);

        for (int x = 0; x < _levelConfig.Width; x++)
        {
            for (int y = 0; y < _levelConfig.Height; y++)
            {
                int index = y * _levelConfig.Width + x;
                int typeIndex = _levelConfig.Cells[index];

                if (typeIndex < 0) continue;

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
        var definition = _levelConfig.Database.GetByIndex(element.TypeIndex);
        ElementView view = Instantiate(definition.Prefab); 

        view.Initialize(element, board, GridToWorld);
        element.View = view;
    }

    private Vector3 GridToWorld(Vector2Int gridPos)
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
