using UnityEngine;

[CreateAssetMenu(menuName = "ElementGame/Element Definition")]
public class ElementsDefinition : ScriptableObject
{
    [SerializeField] private string _elementName;
    [SerializeField, HideInInspector] private int _index;

    [SerializeField] private ElementView _prefab;
    [SerializeField] private Color _editorColor = Color.white;

    public string ElementName => _elementName;
    public int Index => _index;
    public ElementView Prefab => _prefab;
    public Color EditorColor => _editorColor;
}
