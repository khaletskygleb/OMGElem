using UnityEngine;

namespace ElementGame.Data
{
    [CreateAssetMenu(menuName = "ElementGame/Element Definition")]
    public class ElementsDefinition : ScriptableObject
    {
        [SerializeField] private string _elementName;
        [SerializeField, HideInInspector] private int _index;

        [SerializeField] private Core.ElementView _prefab;
        [SerializeField] private Color _editorColor = Color.white;

        public string ElementName => _elementName;
        public int Index => _index;
        public Core.ElementView Prefab => _prefab;
        public Color EditorColor => _editorColor;
    }
}