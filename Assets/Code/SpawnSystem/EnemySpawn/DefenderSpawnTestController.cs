using EnemySystem;
using System.Collections.Generic;
using System;
using UnitSystem.Enum;
using UnitSystem;
using UnityEngine;

namespace SpawnSystem
{
    public class DefenderSpawnTestController : IOnController, IOnUpdate
    {
        private readonly Dictionary<UnitRoleType, IUnitData> _unitSpawnDataCollection = new();

        private readonly UnitFactory _factory;
        private readonly List<Transform> _spawnPoints = new List<Transform>();

        public event Action<IUnit> OnUnitSpawned;

        private int _spawnIndex;
        
        public DefenderSpawnTestController(List<Transform> spawnPoints, SpawnSettings spawnSettings)
        {

            _factory = new EnemyUnitFactory(spawnSettings.UnitsCreationData);

            foreach (var creationData in spawnSettings.UnitsCreationData)
            {
                var unitData = creationData.UnitSettings;
                _unitSpawnDataCollection[unitData.StatsData.Role] = unitData;
            }

            foreach (var spawnPoint in spawnPoints)
            {
                _spawnPoints.Add(spawnPoint);
            }

            _spawnIndex = 0;
        }

        public void SpawnUnit(UnitRoleType unitType)
        {
            if (_spawnIndex >= _spawnPoints.Count)
                return;

            var unit = _factory.CreateUnit(_unitSpawnDataCollection[unitType]);
    
            var spwanPosition = _spawnPoints[_spawnIndex].position;

            unit.SetStartPosition(spwanPosition);

            OnUnitSpawned?.Invoke(unit);

            _spawnIndex++;
        }


        public void OnUpdate(float deltaTime)
        {

        }
    }
}
