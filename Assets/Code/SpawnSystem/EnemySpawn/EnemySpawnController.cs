using System;
using System.Collections.Generic;
using Abstraction;
using EnemySystem;
using Tools;
using UnitSystem;
using UnitSystem.Enum;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpawnSystem
{
    public class EnemySpawnController : ISpawnController
    {
        private readonly SpawnerView _spawner;
        private readonly EnemyPool _pool;

        private readonly List<Transform> _spawnPoints = new List<Transform>();
        
        private TimeRemaining _timer;
        private bool _isTiming;

        public event Action<IUnit> OnUnitSpawned;

        public EnemySpawnController(SpawnerView spawner)
        {
            _spawner = spawner;

            var unitsCreationData = _spawner.SpawnSettings.UnitsCreationData;

            var factory = new EnemyUnitFactory(unitsCreationData);
            
            _pool = new EnemyPool(spawner.PoolHolder, factory, unitsCreationData);
        }

        public void SpawnUnit(UnitType unitType)
        {
            var unit = _pool.GetFromPool(unitType);

            var spawnIndex = Random.Range(0, _spawner.SpawnPoints.Count);
            var spawnPosition = _spawner.SpawnPoints[spawnIndex].position;

            unit.SetStartPosition(spawnPosition);

            OnUnitSpawned?.Invoke(unit);
        }

        public void DespawnUnit(IUnit unit)
        {
            _pool.ReturnToPool(unit);
        }

        public void OnUpdate(float deltaTime)
        {

        }
    }
}
