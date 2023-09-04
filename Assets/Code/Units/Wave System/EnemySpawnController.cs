using System;
using System.Collections.Generic;
using EnemyUnit;
using EnemyUnit.Interfaces;
using UnityEngine;
using WaveSystem.View;
using Object = UnityEngine.Object;

namespace WaveSystem
{
    public class EnemySpawnController : IOnUpdate, IOnFixedUpdate, IDisposable
    {
        private readonly EnemySpawnView _spawnView;
        private readonly EnemyFactory _enemyFactory;
        private readonly EnemyPool _enemyPool;

        private List<IEnemyController> _enemies;

        public EnemySpawnController(
            Damageable primaryTarget,
            Transform spawnerPlacement,
            EnemySpawnerSettings settings, 
            List<EnemyData> enemyDataList)
        {
            _spawnView = Object.Instantiate(settings.SpawnerGO, spawnerPlacement, false);

            _enemyFactory = new EnemyFactory(primaryTarget);
            _enemyPool = new EnemyPool(_spawnView.PoolHolder, _enemyFactory, enemyDataList);
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

            Object.Destroy(_spawnView);
        }
    }
}
