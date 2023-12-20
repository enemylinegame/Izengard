using System;
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
        private readonly IUnitsContainer _unitsContainer;
        private readonly List<UnitCreationData> _unitCreationDataList;

        private readonly UnitViewPool _viewPool;

        private int _nextSpawnPositionsIndex;

        public event Action<IUnit> OnUnitSpawned;

        public DefendersSpawnController(SpawnerView spawner, IUnitsContainer unitsContainer)
        {
            _spawner = spawner;
            _unitsContainer = unitsContainer;

            _unitCreationDataList = _spawner.SpawnSettings.UnitsCreationData;

            _viewPool = new UnitViewPool(spawner.PoolHolder, _unitCreationDataList);

            _nextSpawnPositionsIndex = 0;

            _unitsContainer.OnUnitRemoved += DespawnUnit;
        }

        public void SpawnUnit(IUnitData unitData)
        {
            var unitView = _viewPool.GetFromPool(unitData.Type);

            var unit = new UnitHandler(unitView, unitData);

            unit.SetStartPosition(SelectSpawnPosition());

            _unitsContainer.AddUnit(unit);
            OnUnitSpawned?.Invoke(unit);
        }

        public void SpawnUnit(UnitType type)
        {
            var unitView = _viewPool.GetFromPool(type);

            var unitData
                = _unitCreationDataList.Find(ucd => ucd.Type == type).UnitSettings;

            var unit = new UnitHandler(unitView, unitData);

            unit.SetStartPosition(SelectSpawnPosition());

            _unitsContainer.AddUnit(unit);
            OnUnitSpawned?.Invoke(unit);
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
            if (unit.Stats.Faction != UnitFactionType.Defender)
                return;

            _viewPool.ReturnToPool(unit.View);
        }
    }
}