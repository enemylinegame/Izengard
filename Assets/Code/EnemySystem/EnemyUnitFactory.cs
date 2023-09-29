using UnitSystem;
using UnitSystem.Data;
using System.Collections.Generic;
using UnityEngine;

namespace EnemySystem
{
    public class EnemyUnitFactory : UnitFactory
    {
        private int _enemyId;

        public EnemyUnitFactory(List<UnitCreationData> unitObjectDataList) : base(unitObjectDataList)
        {
            _enemyId = 0;
        }

        public override IUnit CreateMilitiaman(IUnitData unitData)
        {
            return CreateEnemy(unitData);
        }

        public override IUnit CreateHunter(IUnitData unitData)
        {
            return CreateEnemy(unitData);
        }

        private IUnit CreateEnemy(IUnitData unitData)
        {
            var unitPrefab = unitObjectsData[unitData.StatsData.Role];

            var unitGO = Object.Instantiate(unitPrefab);

            var view = unitGO.GetComponent<IUnitView>();

            var unitDefence = new UnitDefenceModel(unitData.DefenceData);
            var unitOffence = new UnitOffenceModel(unitData.OffenceData);

            var model = new UnitModel(
                unitData.StatsData,
                unitDefence,
                unitOffence);

            var unitPriority = 
                new UnitPriorityModel(view.SelfTransform, unitData.UnitPriorities);

            var navigation = 
                new EnemyNavigationModel(view.UnitNavigation, view.SelfTransform.position);

            var unitHandler = 
                new UnitHandler(_enemyId++, view, model, unitPriority, navigation);

            return unitHandler;
        }
    }
}
