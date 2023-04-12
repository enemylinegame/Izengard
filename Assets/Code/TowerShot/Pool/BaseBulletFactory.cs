using Interfaces;
using System;
using UnityEngine;
using Views.BaseUnit;


namespace Models.BaseUnit
{
    public class BaseBulletFactory: IDisposable
    {
        private readonly IPoolController<GameObject> _pool;
        public BaseBulletFactory(IPoolController<GameObject> pool)
        {
            _pool = pool;
        }
        public GameObject CreateBullet(Vector3 whereToPlace)
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