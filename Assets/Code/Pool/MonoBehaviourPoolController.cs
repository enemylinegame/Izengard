using Interfaces;
using System.Collections.Generic;
using UnityEngine;


namespace Controllers.Pool
{
    public class MonoBehaviourPoolController<T> : IPoolController<T> where T : MonoBehaviour
    {
        private readonly List<T> _pool = new List<T>();
        private readonly T _poolObject;
        private readonly Transform _poolHolder;


        public MonoBehaviourPoolController(int startPoolCapacity, T poolObject, Transform poolHolder = null)
        {
            if (poolHolder == null) _poolHolder = new GameObject("Pool_" + poolObject.name).transform;
            else _poolHolder = poolHolder;

            for (int i = 0; i < startPoolCapacity; ++i)
            {
                var instantiatedPoolObject = Object.Instantiate(poolObject, _poolHolder);
                _pool.Add(instantiatedPoolObject);
                instantiatedPoolObject.gameObject.SetActive(false);
            }
            _poolObject = poolObject;
        }

        public T GetObjectFromPool()
        {
            for (int i = 0; i < _pool.Count; ++i)
            {
                if (!_pool[i].gameObject.activeInHierarchy)
                {
                    _pool[i].gameObject.SetActive(true);
                    return _pool[i];
                }
            }

            var poolObject = Object.Instantiate(_poolObject, _poolHolder);
            _pool.Add(poolObject);
            return poolObject;
        }

        public void ReturnObjectInPool(T poolObject)
        {
            poolObject.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            foreach (var obj in _pool) Object.Destroy(obj);
            _pool.Clear();
        }
    }
}