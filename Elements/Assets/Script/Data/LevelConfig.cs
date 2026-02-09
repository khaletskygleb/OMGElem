using UnityEngine;

[CreateAssetMenu(menuName = "ElementGame/Level")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private ElementsDatabase _pieceDatabase;
    [Space]
    [SerializeField] private int _width = 6;
    [SerializeField] private int _height = 6;
    [SerializeField] private int[] _cells;

    public ElementsDatabase Database => _pieceDatabase;
    public int Width => _width;
    public int Height => _height;   
    public int[] Cells => _cells;

    public void Resize()
    {
        _cells = new int[_width * _height];
        for (int i = 0; i < _cells.Length; i++)
            _cells[i] = -1;
    }
}
