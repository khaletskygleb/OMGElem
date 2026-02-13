using UnityEngine;
using ElementGame.Pool;

namespace ElementGame
{
    public class Balloon : APoolableObject
    {
        [SerializeField] private float _minAmplitude = 0.3f;
        [SerializeField] private float _maxAmplitude = 1.2f;
        [SerializeField] private float _minFrequency = 0.5f;
        [SerializeField] private float _maxFrequency = 2f;
        [SerializeField] private float _minInitialPhase = 0f;
        [SerializeField] private float _maxInitialPhase = 10f;

        private float _speed;
        private int _direction;
        private float _leftBound;
        private float _rightBound;

        private float _amplitude;
        private float _frequency;

        private float _baseY;
        private float _time;

        private System.Action<Balloon> _onDespawn;

        public void Initialize(
            Vector3 startPos,
            int direction,
            float speed,
            float leftBound,
            float rightBound,
            System.Action<Balloon> onDespawn)
        {
            transform.position = startPos;

            _direction = direction;
            _speed = speed;
            _leftBound = leftBound;
            _rightBound = rightBound;
            _onDespawn = onDespawn;

            _baseY = startPos.y;

            _amplitude = Random.Range(_minAmplitude, _maxAmplitude);
            _frequency = Random.Range(_minFrequency, _maxFrequency);

            _time = Random.Range(_minInitialPhase, _maxInitialPhase);

        }

        public override void OnReturnToPool()
        {
            _onDespawn?.Invoke(this);
        }

        private void Update()
        {
            _time += Time.deltaTime;

            float newX = transform.position.x + _direction * _speed * Time.deltaTime;

            float yOffset = Mathf.Sin(_time * _frequency) * _amplitude;
            float rotation = Mathf.Sin(_time * _frequency) * 8f;

            Vector3 pos = transform.position;
            pos.x = newX;
            pos.y = _baseY + yOffset;

            transform.position = pos;
            transform.rotation = Quaternion.Euler(0, 0, rotation);

            if (newX > _rightBound + 1f || newX < _leftBound - 1f)
            {
                ReturnToPool();
            }
        }
    }
}