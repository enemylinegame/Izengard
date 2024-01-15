using System;
using System.Collections.Generic;
using UnityEngine;
using UnitSystem;
using UnitSystem.Data;
using UnitSystem.Enum;
using Random = UnityEngine.Random;
using Abstraction;

namespace SpawnSystem
{
    public class DefendersSpawnController : ISpawnController
    {
        private readonly FactionType _faction;

        private readonly SpawnerView _spawner;
        private readonly SpawnCreationController _spawnCreationController;
        private readonly IUnitsContainer _unitsContainer;

        private readonly List<UnitCreationData> _unitCreationDataList;
        private readonly UnitViewPool _viewPool;

        private readonly float _maxSpawnRadius;

        private List<Spawner> _spawnersCollection;

        public event Action<IUnit> OnUnitSpawned;

        public DefendersSpawnController(
            SpawnerView spawner,
            SpawnCreationController spawnCreationController,
            IUnitsContainer unitsContainer)
        {
            _faction = FactionType.Defender;

            _spawner = spawner;
            _spawnCreationController = spawnCreationController;
            _unitsContainer = unitsContainer;

            _spawnersCollection = new List<Spawner>();

            _maxSpawnRadius = _spawner.SpawnSettings.MaxSpawnRadius;
            _unitCreationDataList = _spawner.SpawnSettings.UnitsCreationData;

            _viewPool = new UnitViewPool(spawner.PoolHolder, _unitCreationDataList);

            _spawnCreationController.OnSpawnerCreated += SpawnerCreated;
            _spawnCreationController.OnSpawnerRemoved += SpawnerRemoved;

            _unitsContainer.OnUnitRemoved += DespawnUnit;
        }

        private void SpawnerCreated(Spawner spawner)
        {
            if (spawner.FactionType != FactionType.Defender)
                return;

            _spawnersCollection.Add(spawner);
        }

        private void SpawnerRemoved(Spawner spawner)
        {
            if (spawner.FactionType != FactionType.Defender)
                return;

            _spawnersCollection.Remove(spawner);
        }

        public void SpawnUnit(IUnitData unitData)
        {
            if (_spawnCreationController.SelectedSpawner == null)
                return;

            if (_spawnCreationController.SelectedSpawner.FactionType != _faction)
                return;

            var unitView = _viewPool.GetFromPool(unitData.Type);

            var unit = new UnitHandler(unitView, unitData);

            var spawnPos = GetSpawnPosition(_spawnCreationController.SelectedSpawner);

            unit.SetStartPosition(spawnPos);

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

            var spawnerIndex = Random.Range(0, _spawnersCollection.Count);

            var spawnPos = GetSpawnPosition(_spawnersCollection[spawnerIndex]);

            unit.SetStartPosition(spawnPos);

            _unitsContainer.AddUnit(unit);

            OnUnitSpawned?.Invoke(unit);
        }

        private Vector3 GetSpawnPosition(Spawner spawner)
        {
            var spawnPosition = spawner.SpawnLocation.position;

            return GetPositionInsideRadius(spawnPosition, _maxSpawnRadius);
        }

        private Vector3 GetPositionInsideRadius(Vector3 spawnRootPos, float radius)
        {
            var radiusPos = Random.insideUnitCircle * radius;
            return spawnRootPos + new Vector3(radiusPos.x, 0, radiusPos.y);
        }

        public void DespawnUnit(IUnit unit)
        {
            if (unit.Stats.Faction != FactionType.Defender)
                return;

            _viewPool.ReturnToPool(unit.View);
        }
    }
}