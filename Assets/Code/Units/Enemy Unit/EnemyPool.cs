using System.Collections.Generic;
using EnemyUnit.Interfaces;
using UnityEngine;

namespace EnemyUnit
{
    public class EnemyPool
    {
        private readonly Queue<IEnemyController> _pool;
        private readonly Transform _poolHolder;

        private readonly EnemyData _data;
        private readonly EnemyFactory _enemyFactory;

        public EnemyPool(
            int poolCapacity,
            Transform poolHolder,
            EnemyData data,
            EnemyFactory enemyFactory)
        {
            _pool = new Queue<IEnemyController>();
            _poolHolder = new GameObject("Pool_" + _data.Type.ToString()).transform;
            _poolHolder.parent = poolHolder;

            _data = data;
            _enemyFactory = enemyFactory;

            for (int i = 0; i < poolCapacity; i++)
            {
                var enemy = enemyFactory.CreateEnemy(_data);
                _pool.Enqueue(enemy);
            }
        }

        private IEnemyController InstantiateEnemy(EnemyData data, bool isActive = false)
        {
            var enemyObj = Object.Instantiate(data.Prefab, _poolHolder);

            enemyObj.SetActive(isActive);

            return null;
        }

        public IEnemyController GetFromPool()
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
    }
}
