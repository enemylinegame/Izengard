using Interfaces;
using UnityEngine;

namespace EquipmentSystem
{
    public class BaseELFUnitFactory
    {
        private readonly IPoolController<GameObject> _pool;
        
        public BaseELFUnitFactory(IPoolController<GameObject> pool)
        {
            _pool = pool;
        }
        public GameObject CreateUnit(Vector3 whereToPlace)
        {
            var poolObject = _pool.GetObjectFromPool();
            poolObject.transform.position = whereToPlace;
            return poolObject;
        }
        public void Dispose()
        {
            _pool?.Dispose();
        }
    }
}