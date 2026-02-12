using System.Collections.Generic;
using UnityEngine;

namespace ElementGame.Pool
{
    class Pool
    {
        private readonly APoolableObject _prefab;
        private readonly Stack<APoolableObject> _objects = new();

        public Pool(APoolableObject prefab)
        {
            _prefab = prefab;
        }

        public APoolableObject Get()
        {
            APoolableObject obj = _objects.Count > 0
                ? _objects.Pop()
                : GameObject.Instantiate(_prefab);

            obj.gameObject.SetActive(true);

            obj.OnGet();

            return obj;
        }

        public void Return(APoolableObject obj)
        {
            obj.OnReturnToPool();
            Transform t = obj.transform;
            t.localScale = Vector3.one;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;

            obj.gameObject.SetActive(false);
            _objects.Push(obj);
        }

        public int Count => _objects.Count;
    }
}