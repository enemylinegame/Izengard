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
        private readonly SpawnCreationController _spawnCreationController;
        private readonly IUnitsContainer _unitsContainer;
       
        private readonly List<UnitCreationData> _unitCreationDataList;
        private readonly UnitViewPool _viewPool;

        private List<Spawner> _spawnersCollection;
        private int _nextSpawnPositionsIndex;

        public event Action<IUnit> OnUnitSpawned;

        public DefendersSpawnController(
            SpawnerView spawner,
            SpawnCreationController spawnCreationController,
            IUnitsContainer unitsContainer)
        {
            _spawner = spawner;
            _spawnCreationController = spawnCreationController;
            _unitsContainer = unitsContainer;

            _spawnersCollection = new List<Spawner>();

            _unitCreationDataList = _spawner.SpawnSettings.UnitsCreationData;

            _viewPool = new UnitViewPool(spawner.PoolHolder, _unitCreationDataList);

            _spawnCreationController.OnSpawnerCreated += SpawnerCreated;
            _spawnCreationController.OnSpawnerRemoved += SpawnerRemoved;

            _nextSpawnPositionsIndex = 0;

            _unitsContainer.OnUnitRemoved += DespawnUnit;
        }

        private void SpawnerCreated(Spawner spawner)
        {
            if (spawner.FactionType != UnitFactionType.Defender)
                return;

            _spawnersCollection.Add(spawner);
        }

        private void SpawnerRemoved(Spawner spawner)
        {
            if (spawner.FactionType != UnitFactionType.Defender)
                return;

            _spawnersCollection.Remove(spawner);
        }

        public void SpawnUnit(IUnitData unitData)
        {
            if (_spawnersCollection.Count == 0)
                return;

            var unitView = _viewPool.GetFromPool(unitData.Type);

            var unit = new UnitHandler(unitView, unitData);

            unit.SetStartPosition(SelectSpawnPosition());

            _unitsContainer.AddUnit(unit);
            OnUnitSpawned?.Invoke(unit);
        }

        public void SpawnUnit(UnitType type)
        {
            if (_spawnersCollection.Count == 0)
                return;

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
            if (_nextSpawnPositionsIndex >= _spawnersCollection.Count)
            {
                _nextSpawnPositionsIndex = 0;
            }

            Vector3 spawnPosition 
                = _spawnersCollection[_nextSpawnPositionsIndex].SpawnLocation.position;

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