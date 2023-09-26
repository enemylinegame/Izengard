using System.Collections.Generic;
using Izengard.EnemySystem;
using Izengard.UnitSystem;
using UnityEngine;

namespace Izengard.SpawnSystem
{
    public class EnemySpawnController : IOnController, IOnUpdate
    {
        private readonly UnitFactory _factory;
        private readonly List<Transform> _spawnPoints = new List<Transform>();
        private readonly List<IUnit> _spawnedUnits;

        public List<IUnit> SpawnedUnits => _spawnedUnits;

        public EnemySpawnController(
            List<Transform> spawnPoints, 
            SpawnSettings spawnSettings)
        {
            foreach (var spawnPoint in spawnPoints)
            {
                _spawnPoints.Add(spawnPoint);
            }

            _factory = new EnemyUnitFactory(spawnSettings.UnitsCreationData);

            _spawnedUnits = new List<IUnit>();

            foreach (var creationData in spawnSettings.UnitsCreationData)
            {
                var spawnIndex = Random.Range(0, _spawnPoints.Count);
                var spwanPosition = _spawnPoints[spawnIndex].position;
                var unit = SpawnUnit(creationData.UnitSettings, spwanPosition);

                _spawnedUnits.Add(unit);
            }
        }


        public void OnUpdate(float deltaTime)
        {
            
        }

        public IUnit SpawnUnit(IUnitData unitData, Vector3 spawnPos)
        {
            var unit = _factory.CreateUnit(unitData);

            unit.SetPosition(spawnPos);
           // unit.SetRotation(Vector3.forward);

            return unit;
        }
    }
}
