using UnityEngine;
using Zenject;

namespace ElementGame.Pool
{
    public abstract class APoolableObject : MonoBehaviour
    {
        protected ObjectPoolService Pool;

        [Inject]
        public void Construct(ObjectPoolService pool)
        {
            Pool = pool;
        }

        public void ReturnToPool()
        {
            Pool.Return(this);
        }

        public virtual void OnGet() { }
        public virtual void OnReturnToPool() { }
    }
}

