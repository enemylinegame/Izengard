using System;
using System.Collections.Generic;
using EnemySystem;
using Tools;
using UnitSystem;
using UnitSystem.Enum;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpawnSystem
{
    public class EnemySpawnController : IOnController, IOnUpdate
    {
        private readonly Dictionary<UnitRoleType, IUnitData> _unitSpawnDataCollection = new();

        private readonly EnemyPool _pool;

        private readonly List<Transform> _spawnPoints = new List<Transform>();
        
        private TimeRemaining _timer;
        private bool _isTiming;

        public event Action<IUnit> OnUnitSpawned;

        public EnemySpawnController(
            Transform spawner, 
            List<Transform> spawnPoints, 
            SpawnSettings spawnSettings)
        {

            var factory = new EnemyUnitFactory(spawnSettings.UnitsCreationData);
            _pool = new EnemyPool(spawner, factory, spawnSettings.UnitsCreationData);


            foreach (var creationData in spawnSettings.UnitsCreationData)
            {
                var unitData = creationData.UnitSettings;
                _unitSpawnDataCollection[unitData.StatsData.Role] = unitData;
            }

            foreach (var spawnPoint in spawnPoints)
            {
                _spawnPoints.Add(spawnPoint);
            }
        }

        public void SpawnUnit(UnitRoleType unitType)
        {
            var unit = _pool.GetFromPool(unitType);

            var spawnIndex = Random.Range(0, _spawnPoints.Count);
            var spwanPosition = _spawnPoints[spawnIndex].position;

            unit.SetSpawnPosition(spwanPosition);

            OnUnitSpawned?.Invoke(unit);
        }


        public void OnUpdate(float deltaTime)
        {

        }
    }
}
