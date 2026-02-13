using ElementGame.Data;
using ElementGame.Pool;
using UnityEngine;
using Zenject;

namespace ElementGame
{
    public class BalloonSpawner : MonoBehaviour
    {
        [Header("Spawn Area")]
        [SerializeField] private float _leftX;
        [SerializeField] private float _rightX;
        [SerializeField] private float _minY;
        [SerializeField] private float _maxY;

        [Header("Limits")]
        [SerializeField] private int _maxBalloons = 3;
        [SerializeField] private float _spawnDelayMin = 1f;
        [SerializeField] private float _spawnDelayMax = 3f;

        private ObjectPoolService _pool;
        private BalloonLibrarySO _library;

        private int _activeCount;

        [Inject]
        public void Construct(
            ObjectPoolService pool,
            BalloonLibrarySO library)
        {
            _pool = pool;
            _library = library;
        }

        private void Start()
        {
            StartCoroutine(SpawnLoop());
        }

        private System.Collections.IEnumerator SpawnLoop()
        {
            while (true)
            {
                if (_activeCount < _maxBalloons)
                    SpawnBalloon();

                float delay = Random.Range(_spawnDelayMin, _spawnDelayMax);
                yield return new WaitForSeconds(delay);
            }
        }

        private void SpawnBalloon()
        {
            bool leftToRight = Random.value > 0.5f;

            float spawnY = Random.Range(_minY, _maxY);
            float spawnX = leftToRight ? _leftX : _rightX;
            float speed = Random.Range(0.5f, 2f);

            Balloon prefab = _library.GetRandom();
            if (prefab == null) return;

            var balloon = _pool.Get(prefab);

            balloon.Initialize(
                startPos: new Vector3(spawnX, spawnY, 0f),
                direction: leftToRight ? 1 : -1,
                speed: speed,
                leftBound: _leftX,
                rightBound: _rightX,
                OnBalloonDespawn
            );

            _activeCount++;
        }

        private void OnBalloonDespawn(Balloon balloon)
        {
            _activeCount--;
        }
    }
}