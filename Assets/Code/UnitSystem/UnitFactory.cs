using UnityEngine;
using UnitSystem.Enum;
using UnitSystem.Data;
using System.Collections.Generic;

namespace UnitSystem
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
                UnitType.Militiaman => CreateMilitiaman(unitData),
                UnitType.Hunter => CreateHunter(unitData),
                UnitType.Mage => CreateMage(unitData),
                UnitType.Imp => CreateImp(unitData),
                UnitType.Hound => CreateHound(unitData),
                UnitType.Fiend => CreateFiend(unitData),
                _ => new StubUnit("StubUnitModel was created!. Check Unit Configs")
            };

            return unit;
        }
        public abstract IUnit CreateMilitiaman(IUnitData unitData);
        public abstract IUnit CreateHunter(IUnitData unitData);
        public abstract IUnit CreateMage(IUnitData unitData);
        public abstract IUnit CreateImp(IUnitData unitData);
        public abstract IUnit CreateHound(IUnitData unitData);
        public abstract IUnit CreateFiend(IUnitData unitData);
    }
}
