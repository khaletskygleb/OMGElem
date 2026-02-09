using System.Collections.Generic;
using UnityEngine;

public static class MatchFinder
{
    private static readonly Vector2Int[] _directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    public static List<Cell> FindAllMatches(Board board)
    {
        bool[,] visited = new bool[board.Width, board.Height];
        HashSet<Cell> cellsToRemove = new HashSet<Cell>();

        for (int x = 0; x < board.Width; x++)
            for (int y = 0; y < board.Height; y++)
            {
                if (visited[x, y]) continue;

                Cell startCell = board.GetCell(new Vector2Int(x, y));
                if (startCell.IsEmpty) continue;

                List<Cell> cluster = FloodFillCluster(board, startCell, visited);

                if (ClusterContainsLine(cluster))
                {
                    foreach (Cell cell in cluster)
                        cellsToRemove.Add(cell);
                }
            }

        return new List<Cell>(cellsToRemove);
    }

    private static List<Cell> FloodFillCluster(Board board, Cell startCell, bool[,] visited)
    {
        List<Cell> cluster = new List<Cell>();
        Stack<Cell> stack = new Stack<Cell>();

        int typeIndex = startCell.Element.TypeIndex;

        stack.Push(startCell);
        visited[startCell.Position.x, startCell.Position.y] = true;

        while (stack.Count > 0)
        {
            Cell current = stack.Pop();
            cluster.Add(current);

            foreach (var dir in _directions)
            {
                Vector2Int nextPos = current.Position + dir;

                if (!board.IsInside(nextPos)) continue;
                if (visited[nextPos.x, nextPos.y]) continue;

                Cell neighbor = board.GetCell(nextPos);
                if (neighbor.IsEmpty) continue;
                if (neighbor.Element.TypeIndex != typeIndex) continue;

                visited[nextPos.x, nextPos.y] = true;
                stack.Push(neighbor);
            }
        }

        return cluster;
    }

    private static bool ClusterContainsLine(List<Cell> cluster)
    {
        HashSet<Vector2Int> positions = new HashSet<Vector2Int>();
        foreach (var cell in cluster)
            positions.Add(cell.Position);

        foreach (var cell in cluster)
        {
            Vector2Int pos = cell.Position;

            int horizontal =
                1 +
                CountInDirection(pos, Vector2Int.left, positions) +
                CountInDirection(pos, Vector2Int.right, positions);

            if (horizontal >= 3) return true;

            int vertical =
                1 +
                CountInDirection(pos, Vector2Int.up, positions) +
                CountInDirection(pos, Vector2Int.down, positions);

            if (vertical >= 3) return true;
        }

        return false;
    }

    private static int CountInDirection(Vector2Int start, Vector2Int direction, HashSet<Vector2Int> positions)
    {
        int count = 0;
        Vector2Int current = start + direction;

        while (positions.Contains(current))
        {
            count++;
            current += direction;
        }

        return count;
    }
}
