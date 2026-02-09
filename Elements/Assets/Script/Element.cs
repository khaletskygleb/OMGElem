using UnityEngine;

public class Element
{
    public int TypeIndex { get; }
    public Vector2Int Position { get; internal set; }
    public ElementView View { get; set; }

    public Element(int typeIndex, Vector2Int startPosition)
    {
        TypeIndex = typeIndex;
        Position = startPosition;
    }
}
