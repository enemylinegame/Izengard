using Interfaces;
using System.Collections.Generic;
using UnityEngine;


namespace CombatSystem
{
    public class BulletControllerPool : IPoolController<IBulletController>
    {
        private const float BULLET_SPEED = 3;
        private readonly List<IBulletController> _pool = new List<IBulletController>();
        private readonly Transform _poolHolder;


        public BulletControllerPool(int startPoolCapacity)
        {
            _poolHolder = new GameObject("Pool_Bullet").transform;

            for (int i = 0; i < startPoolCapacity; ++i)
            {
                InstantiateBullet(false);
            }
        }

        private IBulletController InstantiateBullet(bool isActive)
        {
            var bulletController = new BulletController(BULLET_SPEED);
            bulletController.Bullet.transform.parent = _poolHolder;
            bulletController.Bullet.SetActive(isActive);
            _pool.Add(bulletController);
            bulletController.BulletFlightIsOver += ReturnObjectInPool;
            return bulletController;
        }

        public IBulletController GetObjectFromPool()
        {
            for (int i = 0; i < _pool.Count; ++i)
            {
                if (!_pool[i].Bullet.activeInHierarchy)
                {
                    _pool[i].Bullet.SetActive(true);
                    return _pool[i];
                }
            }

            return InstantiateBullet(true);
        }

        public void ReturnObjectInPool(IBulletController obj)
        {
            obj.Bullet.SetActive(false);
        }

        public void Dispose()
        {
            foreach (var bulletController in _pool)
            {
                bulletController.BulletFlightIsOver -= ReturnObjectInPool;
                bulletController.Dispose();
            }
            _pool.Clear();
        }
    }
}