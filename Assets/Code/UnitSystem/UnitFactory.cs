using UnityEngine;
using UnitSystem.Enum;
using UnitSystem.Data;
using System.Collections.Generic;

namespace UnitSystem
{
    public abstract class UnitFactory
    {
        protected readonly Dictionary<UnitRoleType, GameObject> unitObjectsData;

        public UnitFactory(List<UnitCreationData> unitCreationDatas) 
        {
            unitObjectsData = new Dictionary<UnitRoleType, GameObject>();

            for (int i = 0; i < unitCreationDatas.Count; i++) 
            {
                var unitType = unitCreationDatas[i].UnitSettings.StatsData.Role;
                unitObjectsData[unitType] = unitCreationDatas[i].UnitPrefab;
            }
        }

        public IUnit CreateUnit(IUnitData unitData) 
        {
            var unit = unitData.StatsData.Role switch
            {
                UnitRoleType.Militiaman => CreateMilitiaman(unitData),
                UnitRoleType.Hunter => CreateHunter(unitData),
                UnitRoleType.Mage => CreateMage(unitData),
                UnitRoleType.Imp => CreateImp(unitData),
                UnitRoleType.Hound => CreateHound(unitData),
                UnitRoleType.Fiend => CreateFiend(unitData),
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
