using System.Collections.Generic;
using UnityEngine;

namespace ElementGame.Core
{
    public struct FallData
    {
        public Element Element;
        public Vector2Int From;
        public Vector2Int To;
    }

    public static class GravitySystem
    {
        public static List<FallData> CalculateFalls(Board board)
        {
            var result = new List<FallData>();

            for (int x = 0; x < board.Width; x++)
            {
                for (int y = 0; y < board.Height; y++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    Cell cell = board.GetCell(pos);

                    if (cell.IsEmpty) continue;

                    Vector2Int lowest = FindLowestEmpty(board, pos);

                    if (lowest != pos)
                    {
                        result.Add(new FallData
                        {
                            Element = cell.Element,
                            From = pos,
                            To = lowest
                        });
                    }
                }
            }

            return result;
        }

        private static Vector2Int FindLowestEmpty(Board board, Vector2Int start)
        {
            Vector2Int current = start;

            while (true)
            {
                Vector2Int below = current + Vector2Int.down;
                if (!board.IsInside(below)) return current;
                if (!board.GetCell(below).IsEmpty) return current;
                current = below;
            }
        }
    }
}
