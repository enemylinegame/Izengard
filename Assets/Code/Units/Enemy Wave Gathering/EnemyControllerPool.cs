using CombatSystem;
using Interfaces;
using System.Collections.Generic;
using UnityEngine;


namespace Wave
{
    public class EnemyControllerPool : IPoolController<IEnemyController>
    {
        private readonly List<IEnemyController> _pool = new List<IEnemyController>();
        private readonly Transform _poolHolder;
        public readonly EnemySettings Enemy;
        private readonly GeneratorLevelController _generatorLevelController;
        private readonly IEnemyAIController _enemyAIController;
        private readonly IBulletsController _bulletsController;


        public EnemyControllerPool(int startPoolCapacity, EnemySettings enemy, Transform poolHolder, 
            GeneratorLevelController generatorLevelController, IEnemyAIController enemyAIController, IBulletsController bulletsController)
        {
            Enemy = enemy;
            _generatorLevelController = generatorLevelController;
            _enemyAIController = enemyAIController;
            _bulletsController = bulletsController;

            _poolHolder = new GameObject("Pool_" + enemy.Type.ToString()).transform;
            _poolHolder.parent = poolHolder;
            
            for (int i = 0; i < startPoolCapacity; ++i)
            {
                var instantiatedEnemy = InstantiateEnemy(enemy, false);
                _pool.Add(instantiatedEnemy);
            }
        }

        public IEnemyController GetObjectFromPool()
        {
            for (int i = 0; i < _pool.Count; ++i)
            {
                if (!_pool[i].Enemy.RootGameObject.activeInHierarchy)
                {
                    //_pool[i].Enemy.RootGameObject.SetActive(true);
                    _pool[i].SpawnEnemy();
                    return _pool[i];
                }
            }

            var enemy = InstantiateEnemy(Enemy, true);
            _pool.Add(enemy);
            enemy.SpawnEnemy();
            return enemy;
        }

        public void ReturnObjectInPool(IEnemyController enemyController)
        {
            enemyController.Enemy.RootGameObject.SetActive(false);
        }

        private IEnemyController InstantiateEnemy(EnemySettings enemy, bool isActive)
        {
            var instantiatedEnemy = Object.Instantiate(enemy.Prefab, _poolHolder);
            var newEnemy = new EnemyController(enemy, instantiatedEnemy, _generatorLevelController, _enemyAIController, _bulletsController);
            newEnemy.OnEnemyDead += ReturnObjectInPool;
            instantiatedEnemy.SetActive(isActive);
            return newEnemy;
        }

        public void Dispose()
        {
            foreach (var obj in _pool)
            {
                obj.OnEnemyDead -= ReturnObjectInPool;
                obj.Dispose();
            }
            _pool.Clear();
        }
    }
}