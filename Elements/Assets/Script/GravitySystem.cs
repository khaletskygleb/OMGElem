using UnityEngine;

public static class GravitySystem
{
    public static bool Apply(Board board)
    {
        bool anyElementMoved = false;

        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                Cell cell = board.GetCell(position);

                if (cell.IsEmpty) continue;

                Vector2Int lowestPosition = FindLowestEmptyPosition(board, position);

                if (lowestPosition != position)
                {
                    board.MoveElement(position, lowestPosition);
                    anyElementMoved = true;
                }
            }
        }

        return anyElementMoved;
    }

    private static Vector2Int FindLowestEmptyPosition(Board board, Vector2Int startPosition)
    {
        Vector2Int current = startPosition;

        while (true)
        {
            Vector2Int below = current + Vector2Int.down;

            if (!board.IsInside(below))
                return current;

            if (!board.GetCell(below).IsEmpty)
                return current;

            current = below;
        }
    }
}
