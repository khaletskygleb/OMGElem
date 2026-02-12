using System.Collections.Generic;
using UnityEngine;

namespace ElementGame.Pool
{
    public class ObjectPoolService
    {
        private readonly Dictionary<int, Pool> _pools = new();
        private readonly Dictionary<APoolableObject, Pool> _spawned = new();

        public T Get<T>(T prefab) where T : APoolableObject
        {
            int id = prefab.GetInstanceID();

            if (!_pools.TryGetValue(id, out var pool))
            {
                pool = new Pool(prefab);
                _pools[id] = pool;
            }

            var obj = (T)pool.Get();
            _spawned[obj] = pool;

            return obj;
        }

        public void Return(APoolableObject obj)
        {
            if (!_spawned.TryGetValue(obj, out var pool))
            {
                Debug.LogError("Trying to return object that was not spawned from pool!");
                return;
            }

            _spawned.Remove(obj);
            pool.Return(obj);
        }

        public void ReturnAll()
        {
            foreach (var kvp in _spawned)
            {
                kvp.Value.Return(kvp.Key);
            }

            _spawned.Clear();
        }
    }
}