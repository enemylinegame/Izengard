using Interfaces;
using System.Collections.Generic;
using UnityEngine;


namespace Controllers.Pool
{
    public sealed class GameObjectPoolController : IPoolController<GameObject>
    {
        private readonly List<GameObject> _pool = new List<GameObject>();
        private readonly GameObject _poolObject;
        private readonly Transform _poolHolder;


        public GameObjectPoolController(int startPoolCapacity, GameObject poolObject, Transform poolHolder = null)
        {
            if (poolObject == null) return;

            if (poolHolder == null) _poolHolder = new GameObject("Pool_" + poolObject.name).transform;
            else _poolHolder = poolHolder;
            
            for (int i = 0; i < startPoolCapacity; ++i)
            {
                var instantiatedPoolObject = Object.Instantiate(poolObject, _poolHolder);
                _pool.Add(instantiatedPoolObject);
                instantiatedPoolObject.SetActive(false);
            }
            _poolObject = poolObject;
        }

        public GameObject GetObjectFromPool()
        {
            for (int i = 0; i < _pool.Count; ++i)
            {
                if (!_pool[i].activeInHierarchy)
                {
                    _pool[i].SetActive(true);
                    return _pool[i];
                }
            }

            var poolObject = Object.Instantiate(_poolObject, _poolHolder);
            _pool.Add(poolObject);
            return poolObject;
        }

        public void ReturnObjectInPool(GameObject poolObject)
        {
            poolObject.SetActive(false);
        }

        public void Dispose()
        {
            foreach (var obj in _pool) Object.Destroy(obj);
            _pool.Clear();
        }
    }
}