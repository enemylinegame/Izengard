using EnemySystem;
using System;
using System.Collections.Generic;
using UnitSystem;
using UnitSystem.Data;
using UnitSystem.Enum;
using Random = UnityEngine.Random;

namespace SpawnSystem
{
    public class EnemySpawnController : ISpawnController
    {
        private readonly SpawnerView _spawner;
        private readonly IUnitsContainer _unitsContainer;
        private readonly List<UnitCreationData> _unitCreationDataList;

        private readonly EnemyViewPool _viewPool;

        public event Action<IUnit> OnUnitSpawned;

        public EnemySpawnController(SpawnerView spawner, IUnitsContainer unitsContainer)
        {
            _spawner = spawner;
            _unitsContainer = unitsContainer;

            _unitCreationDataList = _spawner.SpawnSettings.UnitsCreationData;

            _viewPool = new EnemyViewPool(spawner.PoolHolder, _unitCreationDataList);

            _unitsContainer.OnUnitRemoved += DespawnUnit;
        }

        public void SpawnUnit(IUnitData unitData)
        {
            var unitView = _viewPool.GetFromPool(unitData.Type);

            var unit = new UnitHandler(unitView, unitData);

            var spawnIndex = Random.Range(0, _spawner.SpawnPoints.Count);
            var spawnPosition = _spawner.SpawnPoints[spawnIndex].position;

            unit.SetStartPosition(spawnPosition);

            _unitsContainer.AddUnit(unit);

            OnUnitSpawned?.Invoke(unit);
        }

        public void SpawnUnit(UnitType type)
        {
            var unitView = _viewPool.GetFromPool(type);

            var unitData 
                = _unitCreationDataList.Find(ucd => ucd.Type == type).UnitSettings;

            var unit = new UnitHandler(unitView, unitData);

            var spawnIndex = Random.Range(0, _spawner.SpawnPoints.Count);
            var spawnPosition = _spawner.SpawnPoints[spawnIndex].position;

            unit.SetStartPosition(spawnPosition);

            _unitsContainer.AddUnit(unit);

            OnUnitSpawned?.Invoke(unit);
        }

        public void DespawnUnit(IUnit unit)
        {
            if (unit.Stats.Faction != UnitFactionType.Enemy)
                return;

            _viewPool.ReturnToPool(unit.View);
        }
    }
}
