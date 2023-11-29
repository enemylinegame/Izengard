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

        private List<UnitCreationData> _unitCreationDataList;

        private int _nextSpawnPositionsIndex;

        public event Action<IUnit> OnUnitSpawned;

        public DefendersSpawnController(SpawnerView spawner, IUnitsContainer unitsContainer)
        {
            _spawner = spawner;
            _unitsContainer = unitsContainer;

            _unitCreationDataList = _spawner.SpawnSettings.UnitsCreationData;
            
            _nextSpawnPositionsIndex = 0;

            _unitsContainer.OnUnitRemoved += DespawnUnit;
        }

        public void SpawnUnit(UnitType unitType)
        {
            UnitCreationData creationData =
                _unitCreationDataList.Find(ucd => ucd.UnitSettings.StatsData.Type == unitType);
            
            if (creationData == null) return;

            GameObject prefab = creationData.UnitPrefab;
            GameObject instance = UnityEngine.Object.Instantiate(prefab);
            IUnitView view = instance.GetComponent<IUnitView>();

            var unit = new UnitHandler(view, creationData.UnitSettings);

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
        }
    }
}