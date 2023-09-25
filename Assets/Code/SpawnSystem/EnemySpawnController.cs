using System.Collections.Generic;
using Izengard.EnemySystem;
using Izengard.UnitSystem;
using UnityEngine;

namespace Izengard.SpawnSystem
{
    public class EnemySpawnController : IOnController, IOnUpdate
    {
        private readonly List<Transform> _spawnPoints = new List<Transform>();

        private readonly List<IUnit> _spawnedUnits;

        private int _spawnIndex;

        public List<IUnit> SpawnedUnits => _spawnedUnits;

        public EnemySpawnController(List<Transform> spawnPoints, SpawnSettings spawnSettings)
        {
            foreach(var spawnPoint in spawnPoints)
            {
                _spawnPoints.Add(spawnPoint);
            }
            
            _spawnedUnits = new List<IUnit>();

            foreach (var unitInfo in spawnSettings.SpawnUnits)
            {
                var unit = SpawnUnit(unitInfo);

                _spawnedUnits.Add(unit);
            }

            _spawnIndex = 0;
        }


        public void OnUpdate(float deltaTime)
        {
            
        }

        public IUnit SpawnUnit(SpawnUnitInfo unitInfo)
        {
            var spawnPointIndex = Random.Range(0, _spawnPoints.Count);

            var unitGO = Object.Instantiate(unitInfo.UnitPrefab, _spawnPoints[spawnPointIndex]);

            var view = unitGO.GetComponent<IUnitView>();

            var unitDefence = new UnitDefenceModel(unitInfo.UnitSettings.DefenceData);
            var unitOffence = new UnitOffenceModel(unitInfo.UnitSettings.OffenceData);
            
            var model = new UnitModel(
                unitInfo.UnitSettings.Faction,
                unitInfo.UnitSettings.StatsData,
                unitDefence,
                unitOffence);

            var navigation = new EnemyNavigationModel(view.UnitNavigation, view.SelfTransform.position);

            var unitHandler = new UnitHandler(_spawnIndex++, view, model, navigation);

            return unitHandler;
        }
    }
}
