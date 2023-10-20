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

        private const int FIRST_UNIT_ID = 10001;
        
        private List<UnitCreationData> _unitCreationDataList;
        private List<Vector3> _spawnPositions;

        public event Action<IUnit> OnUnitSpawned;

        private int _nextSpawnPositionsIndex;
        private int _nextUnitId;
        

        public DefendersSpawnController(List<UnitCreationData> unitCreationsDataList, List<Vector3> spawnPositions)
        {
            _unitCreationDataList = unitCreationsDataList;
            _spawnPositions = spawnPositions;
            _nextUnitId = FIRST_UNIT_ID;
            _nextSpawnPositionsIndex = 0;
        }

        public void SpawnUnit(UnitRoleType unitType)
        {
            UnitCreationData creationData =
                _unitCreationDataList.Find(ucd => ucd.UnitSettings.StatsData.Role == unitType);
            if (creationData == null) return;

            GameObject prefab = creationData.UnitPrefab;
            GameObject instance = GameObject.Instantiate(prefab);
            instance.name = unitType.ToString() + "_" + _nextUnitId.ToString(); 
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
        }

        private Vector3 SelectSpawnPosition()
        {
            if (_nextSpawnPositionsIndex >= _spawnPositions.Count)
            {
                _nextSpawnPositionsIndex = 0;
            }

            Vector3 spawnPosition = _spawnPositions[_nextSpawnPositionsIndex];
            _nextSpawnPositionsIndex++;
            return spawnPosition;
        }
    }
}