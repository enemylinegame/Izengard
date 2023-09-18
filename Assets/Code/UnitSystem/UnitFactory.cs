﻿using UnityEngine;
using Izengard.UnitSystem.Enum;

namespace Izengard.UnitSystem
{
    public class UnitFactory
    {
        public UnitFactory() { }

        public IUnit CreateUnit(IUnitData unitData)
        {
            var unit = unitData.Faction switch
            {
                UnitFactionType.Enemy => CreateEnemy(unitData),
                UnitFactionType.Defender => CreateDefender(unitData),
                _ => new StubUnit("StubUnitModel was created!. Check Unit Configs")
            };

            return unit;
        }

        private IUnit CreateEnemy(IUnitData unitData)
        {
            Debug.Log("Create Enemy");
            return new StubUnit("StubUnitModel was created!. Check UnitFactory");
        }

        private IUnit CreateDefender(IUnitData unitData)
        {
            Debug.Log("Create Defender");
            return new StubUnit("StubUnitModel was created!. Check UnitFactory");
        }
    }
}
