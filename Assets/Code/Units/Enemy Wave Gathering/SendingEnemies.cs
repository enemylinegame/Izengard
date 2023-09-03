using CombatSystem;
using Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Wave.Interfaces;
using Wave.Settings;


namespace Wave
{
    public class SendingEnemies : ISendingEnemys
    {
        public bool IsSending { get; private set; }
        public HashSet<IEnemyController> LifeEnemys { get; private set; }
        private readonly EnemySpawnSettings _enemySpawnSettings;
        private Vector3 _spawnPosition;

        private float _pauseCounter;
        private int _enemyIndex;
        private List<Vector3> _spawnPositions;
        private List<IPoolController<IEnemyController>> _enemys;

        private readonly IEnemyAIController _enemyAIController;
        private readonly TileGenerator _levelTileGenerator;
        private readonly EnemyDestroyObserver _enemyDestroyObserver;


        public SendingEnemies(IEnemyAIController enemyAIController, TileGenerator levelTileGenerator, 
            EnemySpawnSettings enemySpawnSettings, EnemyDestroyObserver enemyDestroyObserver)
        {
            _enemySpawnSettings = enemySpawnSettings;//Resources.Load<EnemySpawnSettings>(nameof(EnemySpawnSettings));
            LifeEnemys = new HashSet<IEnemyController>();

            _enemyAIController = enemyAIController;
            _levelTileGenerator = levelTileGenerator;
            _enemyDestroyObserver = enemyDestroyObserver;
        }

        public void OnUpdate(float deltaTime)
        {
            if (IsSending) StartSendingEnemys(deltaTime);
        }

        public void SendEnemys(List<IPoolController<IEnemyController>> enemys, List<Vector3> spawnPositions)
        {
            IsSending = true;
            _enemys = enemys;
            _spawnPositions = spawnPositions;
            _enemyIndex = 0;
        }

        private void StartSendingEnemys(float deltaTime)
        {
            _pauseCounter -= deltaTime;
            if (_pauseCounter <= 0)
            {
                _pauseCounter += _enemySpawnSettings.PauseBetweenSpawn;

                int spawnPositionIndex = 0;
                while (spawnPositionIndex < _spawnPositions.Count)
                {
                    var enemy = SendEnemy(_enemys[_enemyIndex], _spawnPositions[spawnPositionIndex]);
                    LifeEnemys.Add(enemy);

                    spawnPositionIndex++;
                    _enemyIndex++;
                    if (_enemyIndex >= _enemys.Count)
                    {
                        IsSending = false;
                        break;
                    }
                }
            }
        }

        private IEnemyController SendEnemy(IPoolController<IEnemyController> pool, Vector3 spawnPosition)
        {
            var enemy = pool.GetObjectFromPool();
            enemy.OnEnemyDead += OnEnemyDead;
            SetEnemyNavMesh(enemy, spawnPosition);
            return enemy;
        }

        private void OnEnemyDead(IEnemyController enemy)
        {
            LifeEnemys.Remove(enemy);
            enemy.OnEnemyDead -= OnEnemyDead;
            _enemyDestroyObserver.EnemyDestroyed(enemy.Enemy);
        }

        private void SetEnemyNavMesh(IEnemyController enemy, Vector3 spawnPosition)
        {
            enemy.Enemy.RootGameObject.transform.position = spawnPosition;
            enemy.Enemy.RootGameObject.SetActive(true);
            var navMesh = enemy.Enemy.RootGameObject.GetComponent<NavMeshAgent>();
            navMesh.enabled = true;
            if (navMesh.isOnNavMesh) navMesh.ResetPath();
            navMesh.Warp(spawnPosition);
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            foreach (var fixedUpdate in LifeEnemys)
            {
                fixedUpdate.OnFixedUpdate(fixedDeltaTime);
            }
        }
    }
}