using EquipmentSystem;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Controllers.Pool
{
    public class ItemsPoolController : IDisposable
    {
        private readonly Dictionary<ItemModel, GameObjectPoolController> _pools = new Dictionary<ItemModel, GameObjectPoolController>();
        private readonly Transform _baseHolder;


        public ItemsPoolController()
        {
            _baseHolder = new GameObject("ItemsPool").transform;
        }

        public GameObject GetObjectFromPool(ItemModel item)
        {
            if (_pools.ContainsKey(item)) return _pools[item].GetObjectFromPool();
            else
            {
                var poolHolder = new GameObject(item.ItemObject.name).transform;
                poolHolder.SetParent(_baseHolder);
                _pools[item] = new GameObjectPoolController(0, item.ItemObject, poolHolder);
            }
            return _pools[item].GetObjectFromPool();
        }

        public void ReturnObjectInPool(GameObject poolObject)
        {
            poolObject.SetActive(false);
        }

        public void Dispose()
        {
            foreach (var pool in _pools.Values) pool.Dispose();
            _pools.Clear();
        }
    }
}