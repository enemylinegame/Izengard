using UnityEngine;
using Izengard.UnitSystem.Enum;
using Izengard.UnitSystem.Data;
using System.Collections.Generic;

namespace Izengard.UnitSystem
{
    public abstract class UnitFactory
    {
        protected readonly Dictionary<UnitType, GameObject> unitObjectsData;

        public UnitFactory(List<UnitCreationData> unitCreationDatas) 
        {
            unitObjectsData = new Dictionary<UnitType, GameObject>();

            for (int i = 0; i < unitCreationDatas.Count; i++) 
            {
                var unitType = unitCreationDatas[i].UnitSettings.StatsData.Type;
                unitObjectsData[unitType] = unitCreationDatas[i].UnitPrefab;
            }
        }

        public IUnit CreateUnit(IUnitData unitData) 
        {
            var unit = unitData.StatsData.Type switch
            {
                UnitType.Melee => CreateMeleeUnit(unitData),
                UnitType.Range => CreateRangeUnit(unitData),
                _ => new StubUnit("StubUnitModel was created!. Check Unit Configs")
            };

            return unit;
        }

        public abstract IUnit CreateMeleeUnit(IUnitData unitData);
        public abstract IUnit CreateRangeUnit(IUnitData unitData);
    }
}
