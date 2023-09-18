using System;
using System.Collections.Generic;
using EnemyUnit.Interfaces;
using UnityEngine;

namespace EnemyUnit
{
    public class EnemyPool : IDisposable
    {
        private readonly Queue<IEnemyController> _pool;
        private readonly Transform _poolGO;

        private readonly EnemyData _data;
        private readonly EnemyFactory _enemyFactory;

        public EnemyPool(
            Transform poolHolder, 
            EnemyFactory enemyFactory, 
            List<EnemyData> enemyDataList)
        {
            _pool = new Queue<IEnemyController>();
            _poolGO = new GameObject("Enemy_Pool").transform;
            _poolGO.parent = poolHolder;

            _enemyFactory = enemyFactory;

            foreach (var enemydata in enemyDataList)
            {
                for (int i = 0; i < enemydata.MaxUnitsInPool; i++)
                {
                    var enemy = enemyFactory.CreateEnemy(enemydata);
                    _pool.Enqueue(enemy);
                }
            }
        }

        /* private IEnemyController InstantiateEnemy(EnemyData data, bool isActive = false)
         {
             var enemyObj = Object.Instantiate(data.Prefab, _poolHolder);

             enemyObj.SetActive(isActive);

             return null;
         }*/

        public IEnemyController GetFromPool(EnemyType type)
        {
            if (_pool.Count > 0)
            {
                var enemy = _pool.Dequeue();
                return enemy;
            }
            else
            {
                var enemy = _enemyFactory.CreateEnemy(_data);
                return enemy;
            }
        }

        public void ReturnToPool(IEnemyController enemy)
        {
            _pool.Enqueue(enemy);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
