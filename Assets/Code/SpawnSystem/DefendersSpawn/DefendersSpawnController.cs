using System;
using System.Collections.Generic;
using UnityEngine;
using Abstraction;
using UnitSystem;
using UnitSystem.Data;
using UnitSystem.Enum;
using UnitSystem.Model;


namespace BattleSystem
{
    public class DefendersSpawnController
    {
        
        private List<UnitCreationData> _unitCreationDataList;
        private List<Vector3> _spawnPositions;

        public event Action<IUnit> OnUnitSpawned;

        private int _nextSpawnPositionsIndex;

        
        public DefendersSpawnController(List<UnitCreationData> unitCreationsDataList, List<Vector3> spawnPositions)
        {
            _unitCreationDataList = unitCreationsDataList;
            _spawnPositions = spawnPositions;
            _nextSpawnPositionsIndex = 0;
        }

        public void SpawnUnit(UnitType unitType)
        {
            UnitCreationData creationData =
                _unitCreationDataList.Find(ucd => ucd.UnitSettings.StatsData.Type == unitType);
            if (creationData == null) return;

            GameObject prefab = creationData.UnitPrefab;
            GameObject instance = GameObject.Instantiate(prefab);
            IUnitView view = instance.GetComponent<IUnitView>();

            var unitStats = new UnitStatsModel(creationData.UnitSettings.StatsData);
            var unitDefence = new UnitDefenceModel(creationData.UnitSettings.DefenceData);
            var unitOffence = new UnitOffenceModel(creationData.UnitSettings.OffenceData);
            var navigation = new UnitNavigationModel(view.UnitNavigation, view.SelfTransform.position);
            var priorities = new UnitPriorityModel(creationData.UnitSettings.UnitPriorities);
            var unitHandler = new UnitHandler(view, unitStats, unitDefence, unitOffence, navigation, priorities);
            
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