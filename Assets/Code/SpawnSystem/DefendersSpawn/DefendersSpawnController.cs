using System;
using System.Collections.Generic;
using EnemySystem;
using UnitSystem;
using UnitSystem.Data;
using UnitSystem.Enum;
using UnitSystem.Model;
using UnityEngine;

namespace BattleSystem
{
    public class DefendersSpawnController
    {

        private const int FIRTS_UNIT_ID = 10001;
        
        private List<UnitCreationData> _unitCreationDataList;
        private List<Vector3> _spawnPositions;

        public event Action<IUnit> OnUnitSpawned;

        private int _nextSpawnPositionsIndex;
        private int _nextUnitId;
        

        public DefendersSpawnController(List<UnitCreationData> unitCreationsDataList, List<Vector3> spawnPositions)
        {
            _unitCreationDataList = unitCreationsDataList;
            _spawnPositions = spawnPositions;
            _nextUnitId = FIRTS_UNIT_ID;
            _nextSpawnPositionsIndex = 0;
        }

        public void SpawnUnit(UnitRoleType unitType)
        {
            UnitCreationData creationData =
                _unitCreationDataList.Find(ucd => ucd.UnitSettings.StatsData.Role == unitType);
            if (creationData == null) return;

            GameObject prefab = creationData.UnitPrefab;
            GameObject instance = GameObject.Instantiate(prefab);
            IUnitView view = instance.GetComponent<IUnitView>();

            var unitStats = new UnitStatsModel(creationData.UnitSettings.StatsData);
            var unitDefence = new UnitDefenceModel(creationData.UnitSettings.DefenceData);
            var unitOffence = new UnitOffenceModel(creationData.UnitSettings.OffenceData);
            var navigation = new EnemyNavigationModel(view.UnitNavigation, view.SelfTransform.position);
            var priorities = new UnitPriorityModel(creationData.UnitSettings.UnitPriorities);
            var unitHandler = 
                new UnitHandler(_nextUnitId++, view, unitStats, unitDefence, unitOffence, navigation, priorities);
            
            unitHandler.SetStartPosition(SelectSpawnPosition());
            
            OnUnitSpawned?.Invoke(unitHandler);
            /*
            if (_spawnIndex >= _spawnPoints.Count)
                return;

            var unit = _factory.CreateUnit(_unitSpawnDataCollection[unitType]);
    
            var spwanPosition = _spawnPoints[_spawnIndex].position;

            unit.SetSpawnPosition(spwanPosition);

            OnUnitSpawned?.Invoke(unit);

            _spawnIndex++;
            */
        }

        private Vector3 SelectSpawnPosition()
        {
            if (_nextSpawnPositionsIndex > _spawnPositions.Count)
            {
                _nextSpawnPositionsIndex = 0;
            }

            Vector3 spawnPosition = _spawnPositions[_nextSpawnPositionsIndex];
            _nextSpawnPositionsIndex++;
            return spawnPosition;
        }
        
        
        /*
        private IUnit CreateEnemy(IUnitData unitData)
        {
            var unitPrefab = unitObjectsData[unitData.StatsData.Role];

            var unitGO = Object.Instantiate(unitPrefab);

            var view = unitGO.GetComponent<IUnitView>();

            var unitStats = new UnitStatsModel(unitData.StatsData);

            var unitDefence = new UnitDefenceModel(unitData.DefenceData);

            var unitOffence = new UnitOffenceModel(unitData.OffenceData);

            var navigation =
                new EnemyNavigationModel(view.UnitNavigation, view.SelfTransform.position);

            var priorities = new UnitPriorityModel(unitData.UnitPriorities);

            var unitHandler = 
                new UnitHandler(_enemyId++, view, unitStats, unitDefence, unitOffence, navigation, priorities);

            return unitHandler;
        }
        */
        
    }
}