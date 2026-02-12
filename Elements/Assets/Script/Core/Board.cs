using System;
using UnityEngine;

namespace ElementGame.Core
{
    public class Board
    {
        public event Action<Element> OnElementSpawned;
        public event Action<Element, Vector2Int, Vector2Int> OnElementMoved;
        public event Action<Element> OnElementRemoved;

        public int Width { get; }
        public int Height { get; }

        private readonly Cell[,] cells;

        public Board(int width, int height)
        {
            Width = width;
            Height = height;

            cells = new Cell[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    cells[x, y] = new Cell(new Vector2Int(x, y));
        }

        public bool IsInside(Vector2Int position)
            => position.x >= 0 && position.x < Width &&
               position.y >= 0 && position.y < Height;

        public Cell GetCell(Vector2Int position)
            => cells[position.x, position.y];

        private void SetElementInternal(Vector2Int position, Element element)
        {
            cells[position.x, position.y].Element = element;

            if (element != null)
                element.Position = position;
        }

        public void SpawnElement(Vector2Int position, Element element)
        {
            SetElementInternal(position, element);
            OnElementSpawned?.Invoke(element);
        }

        public void MoveElement(Vector2Int from, Vector2Int to)
        {
            Cell fromCell = GetCell(from);
            Cell toCell = GetCell(to);

            Element element = fromCell.Element;
            if (element == null) return;

            fromCell.Element = null;
            toCell.Element = element;

            Vector2Int oldPos = element.Position;
            element.Position = to;

            OnElementMoved?.Invoke(element, oldPos, to);
        }

        public void SwapElements(Vector2Int firstCell, Vector2Int secondCell)
        {
            Element first = GetCell(firstCell).Element;
            Element second = GetCell(secondCell).Element;

            GetCell(firstCell).Element = second;
            GetCell(secondCell).Element = first;

            if (first != null)
            {
                Vector2Int old = first.Position;
                first.Position = secondCell;
                OnElementMoved?.Invoke(first, old, secondCell);
            }

            if (second != null)
            {
                Vector2Int old = second.Position;
                second.Position = firstCell;
                OnElementMoved?.Invoke(second, old, firstCell);
            }
        }

        public void ClearCell(Vector2Int position)
        {
            Element element = GetCell(position).Element;
            SetElementInternal(position, null);

            if (element != null)
                OnElementRemoved?.Invoke(element);
        }

        public bool HasAnyElements()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (!cells[x, y].IsEmpty)
                        return true;

            return false;
        }
    }
}