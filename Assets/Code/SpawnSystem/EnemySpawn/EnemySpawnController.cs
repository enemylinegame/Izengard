using EnemySystem;
using System;
using UnitSystem;
using UnitSystem.Enum;
using Random = UnityEngine.Random;

namespace SpawnSystem
{
    public class EnemySpawnController : ISpawnController
    {
        private readonly SpawnerView _spawner;
        private readonly IUnitsContainer _unitsContainer;

        private readonly EnemyPool _pool;

        public event Action<IUnit> OnUnitSpawned;

        public EnemySpawnController(SpawnerView spawner, IUnitsContainer unitsContainer)
        {
            _spawner = spawner;
            _unitsContainer = unitsContainer;

            var unitsCreationData = _spawner.SpawnSettings.UnitsCreationData;

            var factory = new EnemyUnitFactory(unitsCreationData);
            
            _pool = new EnemyPool(spawner.PoolHolder, factory, unitsCreationData);

            _unitsContainer.OnUnitRemoved += DespawnUnit;
        }

        public void SpawnUnit(IUnitData unitData)
        {
            
        }

        public void SpawnUnit(UnitType unitType)
        {
            var unit = _pool.GetFromPool(unitType);

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

            _pool.ReturnToPool(unit);
        }
    }
}
