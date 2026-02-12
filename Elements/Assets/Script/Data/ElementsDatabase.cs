using UnityEngine;

namespace ElementGame.Data
{
    [CreateAssetMenu(menuName = "ElementGame/Element Database")]
    public class ElementsDatabase : ScriptableObject
    {
        [SerializeField] private ElementsDefinition[] _elements;

        public ElementsDefinition[] Elements => _elements;

        public ElementsDefinition GetByIndex(int i)
        {
            if (i < 0 || i >= _elements.Length) return null;
            return _elements[i];
        }
    }
}