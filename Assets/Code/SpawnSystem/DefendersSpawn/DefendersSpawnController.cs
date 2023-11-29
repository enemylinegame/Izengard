﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnitSystem;
using UnitSystem.Data;
using UnitSystem.Enum;


namespace SpawnSystem
{
    public class DefendersSpawnController : ISpawnController
    {
        private readonly SpawnerView _spawner;

        private List<UnitCreationData> _unitCreationDataList;

        private int _nextSpawnPositionsIndex;

        public event Action<IUnit> OnUnitSpawned;

        public DefendersSpawnController(SpawnerView spawner)
        {
            _spawner = spawner;

            _unitCreationDataList = _spawner.SpawnSettings.UnitsCreationData;
            
            _nextSpawnPositionsIndex = 0;
        }

        public void SpawnUnit(UnitType unitType)
        {
            UnitCreationData creationData =
                _unitCreationDataList.Find(ucd => ucd.UnitSettings.StatsData.Type == unitType);
            
            if (creationData == null) return;

            GameObject prefab = creationData.UnitPrefab;
            GameObject instance = UnityEngine.Object.Instantiate(prefab);
            IUnitView view = instance.GetComponent<IUnitView>();

            var unitHandler = new UnitHandler(view, creationData.UnitSettings);

            unitHandler.SetStartPosition(SelectSpawnPosition());
            
            OnUnitSpawned?.Invoke(unitHandler);
        }

        private Vector3 SelectSpawnPosition()
        {
            if (_nextSpawnPositionsIndex >= _spawner.SpawnPoints.Count)
            {
                _nextSpawnPositionsIndex = 0;
            }

            Vector3 spawnPosition 
                = _spawner.SpawnPoints[_nextSpawnPositionsIndex].position;
            
            _nextSpawnPositionsIndex++;
            return spawnPosition;
        }
        public void DespawnUnit(IUnit unit)
        {

        }

        public void OnUpdate(float deltaTime) 
        {

        }    
    }
}