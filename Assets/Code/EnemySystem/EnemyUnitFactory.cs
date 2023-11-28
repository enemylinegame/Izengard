using UnitSystem;
using UnitSystem.Data;
using System.Collections.Generic;
using Abstraction;
using UnityEngine;
using UnitSystem.Model;


namespace EnemySystem
{
    public class EnemyUnitFactory : UnitFactory
    {
        private readonly IIdGenerator _idGenerator;

        public EnemyUnitFactory(List<UnitCreationData> unitObjectDataList, IIdGenerator idGenerator) 
            : base(unitObjectDataList)
        {
            _idGenerator = idGenerator;
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

            var unitGO = Object.Instantiate(unitPrefab);

            var view = unitGO.GetComponent<IUnitView>();

            var unitStats = new UnitStatsModel(unitData.StatsData);

            var unitDefence = new UnitDefenceModel(unitData.DefenceData);

            var unitOffence = new UnitOffenceModel(unitData.OffenceData);

            var navigation =
                new EnemyNavigationModel(view.UnitNavigation, view.SelfTransform.position);

            var priorities = new UnitPriorityModel(unitData.UnitPriorities);

            var unitHandler = 
                new UnitHandler(_idGenerator.GetNext(), view, unitStats, unitDefence, unitOffence, navigation, priorities);

            return unitHandler;
        }
    }
}
