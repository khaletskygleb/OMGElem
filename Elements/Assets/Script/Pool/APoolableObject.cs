using UnityEngine;

namespace ElementGame.Pool
{
    public abstract class APoolableObject : MonoBehaviour
    {
        private ObjectPoolService _pool;

        public void SetPool(ObjectPoolService pool)
        {
            _pool = pool;
        }

        public void ReturnToPool()
        {
            if (_pool == null)
            {
                Debug.LogError($"{name} has no pool reference!");
                return;
            }

            _pool.Return(this);
        }

        public virtual void OnGet() { }
        public virtual void OnReturnToPool() { }
    }
}
