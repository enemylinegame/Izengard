using System;
using System.Collections.Generic;
using Izengard.EnemySystem;
using Izengard.UnitSystem;
using Izengard.UnitSystem.Enum;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Izengard.SpawnSystem
{
    public class EnemySpawnController : IOnController, IOnUpdate
    {
        private readonly Dictionary<UnitType, IUnitData> _unitSpawnDataCollection = new();

        private readonly UnitFactory _factory;
        private readonly List<Transform> _spawnPoints = new List<Transform>();

        public event Action<IUnit> OnUnitSpawned;

        public EnemySpawnController(
            List<Transform> spawnPoints,
            SpawnSettings spawnSettings)
        {

            _factory = new EnemyUnitFactory(spawnSettings.UnitsCreationData);

            foreach (var creationData in spawnSettings.UnitsCreationData)
            {
                var unitData = creationData.UnitSettings;
                _unitSpawnDataCollection[unitData.StatsData.Type] = unitData;
            }

            foreach (var spawnPoint in spawnPoints)
            {
                _spawnPoints.Add(spawnPoint);
            }
        }

        public void SpawnUnit(UnitType unitType)
        {
            var unit = _factory.CreateUnit(_unitSpawnDataCollection[unitType]);

            var spawnIndex = Random.Range(0, _spawnPoints.Count);
            var spwanPosition = _spawnPoints[spawnIndex].position;

            unit.SetPosition(spwanPosition);

            OnUnitSpawned?.Invoke(unit);
        }


        public void OnUpdate(float deltaTime)
        {

        }
    }
}
