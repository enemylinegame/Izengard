using UnitSystem;
using UnitSystem.Data;
using System.Collections.Generic;
using UnityEngine;
using UnitSystem.Model;


namespace EnemySystem
{
    public class EnemyUnitFactory : UnitFactory
    {
        public EnemyUnitFactory(List<UnitCreationData> unitObjectDataList)
            : base(unitObjectDataList)
        {

        }

        public override IUnit CreateMilitiaman(IUnitData unitData)
        {
            return CreateEnemy(unitData);
        }

        public override IUnit CreateHunter(IUnitData unitData)
        {
            return CreateEnemy(unitData);
        }

        public override IUnit CreateMage(IUnitData unitData)
        {
            return CreateEnemy(unitData);
        }

        public override IUnit CreateImp(IUnitData unitData)
        {
            return CreateEnemy(unitData);
        }

        public override IUnit CreateHound(IUnitData unitData)
        {
            return CreateEnemy(unitData);
        }
        public override IUnit CreateFiend(IUnitData unitData)
        {
            return CreateEnemy(unitData);
        }

        private IUnit CreateEnemy(IUnitData unitData)
        {
            var unitPrefab = unitObjectsData[unitData.StatsData.Type];

            var unitGO =
                Object.Instantiate(unitPrefab);

            var view =
                unitGO.GetComponent<IUnitView>();

            if (view == null)
            {
                return new StubUnit("Error on create. Can't find view component");
            }

            var unitStats = new UnitStatsModel(unitData.StatsData);

            var unitDefence = new UnitDefenceModel(unitData.DefenceData);

            var unitOffence = new UnitOffenceModel(unitData.OffenceData);

            var navigation = new UnitNavigationModel(view.UnitNavigation, view.SelfTransform.position);

            var priorities = new UnitPriorityModel(unitData.UnitPriorities);

            var unitHandler = new UnitHandler(view, unitStats, unitDefence, unitOffence, navigation, priorities);

            return unitHandler;
        }
    }
}
