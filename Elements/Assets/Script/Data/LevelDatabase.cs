using UnityEngine;

namespace ElementGame.Data
{
    [CreateAssetMenu(menuName = "ElementGame/Level/Level Database")]
    public class LevelDatabase : ScriptableObject
    {
        [SerializeField] private LevelConfig[] _levels;

        public LevelConfig GetLevel(int levelIndex)
        {
            if (_levels == null || _levels.Length == 0)
            {
                Debug.LogError("LevelDatabase: No levels assigned!");
                return null;
            }

            int wrappedIndex = Mathf.Abs(levelIndex) % _levels.Length;
            return _levels[wrappedIndex];
        }

        public int LevelCount => _levels.Length;
    }
}