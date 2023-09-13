using System;
using System.Collections.Generic;
using EnemyUnit;
using EnemyUnit.Interfaces;
using UnityEngine;
using WaveSystem.View;
using Object = UnityEngine.Object;

namespace SpawnSystem
{
    public class EnemySpawnController : IOnUpdate, IOnFixedUpdate, IDisposable
    {
        private readonly EnemySpawnView _spawnView;
        private readonly EnemyFactory _enemyFactory;
        private readonly EnemyPool _enemyPool;

        private readonly PosibleSpawnPointsFinder _pointsFinder;

        private Dictionary<int, IEnemyController> _enemies;

        public EnemySpawnController(
            Damageable primaryTarget,
            Transform spawnerPlacement,
            EnemySpawnerSettings settings, 
            List<EnemyData> enemyDataList,
            PosibleSpawnPointsFinder pointsFinder)
        {
            _spawnView = Object.Instantiate(settings.SpawnerGO, spawnerPlacement, false);

            _enemyFactory = new EnemyFactory(primaryTarget);
            _enemyPool = new EnemyPool(_spawnView.PoolHolder, _enemyFactory, enemyDataList);

            _pointsFinder = pointsFinder;
        }

        public void SpawnEnemy(EnemyType enemyType)
        {
            var enemy = _enemyPool.GetFromPool(enemyType);
            
            enemy.OnDeath += OnEnemyDestroy;

            
            _enemies[enemy.Index] = enemy;
        }

        public void OnEnemyDestroy(int enemyId) 
        {
            var enemy = _enemies[enemyId];
            
            enemy.OnDeath -= OnEnemyDestroy;
            
            _enemyPool.ReturnToPool(enemy);
            _enemies.Remove(enemyId);
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var enemy in _enemies)
            {
                enemy.Value.OnUpdate(deltaTime);
            }
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            foreach (var enemy in _enemies)
            {
                enemy.Value.OnFixedUpdate(fixedDeltaTime);
            }
        }

        public void Dispose()
        {
            foreach (var enemy in _enemies)
            {
                enemy.Value?.Dispose();
            }

            _enemies.Clear();

            _enemyPool?.Dispose();

            Object.Destroy(_spawnView);
        }
    }
}
