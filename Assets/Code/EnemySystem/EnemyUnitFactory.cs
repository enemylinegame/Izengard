using UnitSystem;
using UnitSystem.Data;
using System.Collections.Generic;
using UnityEngine;
using UnitSystem.Model;

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
            throw new System.NotImplementedException();
        }

        public override IUnit CreateHunter(IUnitData unitData)
        {
            throw new System.NotImplementedException();
        }

        public override IUnit CreateMage(IUnitData unitData)
        {
            throw new System.NotImplementedException();
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
            var unitPrefab = unitObjectsData[unitData.StatsData.Role];

            var unitGO = Object.Instantiate(unitPrefab);

            var view = unitGO.GetComponent<IUnitView>();

            var model = new UnitStatsModel(unitData.StatsData);
            
            var unitDefence = new UnitDefenceModel(unitData.DefenceData);

            var unitOffence = new UnitOffenceModel(unitData.OffenceData);

            var navigation = 
                new EnemyNavigationModel(view.UnitNavigation, view.SelfTransform.position);

            var unitHandler = new UnitHandler(
                _enemyId++, 
                view, 
                model, 
                unitDefence, 
                unitOffence, 
                navigation);

            return unitHandler;
        }
    }
}
