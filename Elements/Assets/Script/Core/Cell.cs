using UnityEngine;

namespace ElementGame.Core
{
    public class Cell
    {
        public Vector2Int Position { get; }
        public Element Element { get; internal set; }
        public bool IsEmpty => Element == null;

        public Cell(Vector2Int position)
        {
            Position = position;
        }
    }
}