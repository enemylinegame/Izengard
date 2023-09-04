using System;
using System.Collections.Generic;
using EnemyUnit;
using EnemyUnit.Interfaces;
using WaveSystem.View;

namespace WaveSystem
{
    public class EnemySpawnController : IOnUpdate, IOnFixedUpdate, IDisposable
    {
        private readonly EnemyFactory _enemyFactory;
        private readonly EnemyPool _enemyPool;

        private List<IEnemyController> _enemies;

        public EnemySpawnController(
            Damageable primaryTarget,
            EnemySpawnView view, 
            List<EnemyData> enemyDataList)
        {
            _enemyFactory = new EnemyFactory(primaryTarget);
            _enemyPool = new EnemyPool(view.PoolHolder, _enemyFactory, enemyDataList);
        }

        public void SpawnEnemy(EnemyType enemyType)
        {
            var enemy = _enemyPool.GetFromPool(enemyType);

            _enemies.Add(enemy);
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var enemy in _enemies)
            {
                enemy.OnUpdate(deltaTime);
            }
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            foreach (var enemy in _enemies)
            {
                enemy.OnFixedUpdate(fixedDeltaTime);
            }
        }

        public void Dispose()
        {
            foreach (var enemy in _enemies)
            {
                enemy?.Dispose();
            }

            _enemies.Clear();
        }
    }
}
