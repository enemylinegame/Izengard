using UnityEngine;
using Izengard.Tools;

namespace Izengard.Units
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
                _ => new StubUnitModel(unitData, "StubUnitModel was created!. Check Unit Configs")
            };

            return unit;
        }

        private IUnit CreateEnemy(IUnitData unitData)
        {
            Debug.Log("Create Enemy");
            return new StubUnitModel(unitData, "StubUnitModel was created!. Check UnitFactory");
        }

        private IUnit CreateDefender(IUnitData unitData)
        {
            Debug.Log("Create Defender");
            return new StubUnitModel(unitData, "StubUnitModel was created!. Check UnitFactory");
        }
    }
}
