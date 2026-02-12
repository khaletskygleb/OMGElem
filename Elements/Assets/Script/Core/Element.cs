using UnityEngine;

namespace ElementGame.Core
{
    public enum ElementState
    {
        Idle,
        Moving,
        Falling,
        Destroying
    }

    public class Element
    {
        public int TypeIndex { get; }
        public Vector2Int Position { get; internal set; }
        public ElementView View { get; set; }
        public ElementState State { get; set; } = ElementState.Idle;
        public bool IsInteractable => State == ElementState.Idle;

        public Element(int typeIndex, Vector2Int startPosition)
        {
            TypeIndex = typeIndex;
            Position = startPosition;
        }
    }
}