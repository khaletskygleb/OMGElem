using UnityEngine;

namespace ElementGame.Data
{
    [CreateAssetMenu(menuName = "ElementGame/Decorations/BalloonLibrary")]
    public class BalloonLibrarySO : ScriptableObject
    {
        [SerializeField] private Balloon[] _balloonPrefabs;

        public Balloon GetRandom()
        {
            if (_balloonPrefabs == null || _balloonPrefabs.Length == 0)
            {
                Debug.LogError("BalloonLibrary is empty!");
                return null;
            }

            int index = Random.Range(0, _balloonPrefabs.Length);
            return _balloonPrefabs[index];
        }
    }

}