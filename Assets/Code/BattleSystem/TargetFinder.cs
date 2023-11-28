using System.Collections.Generic;
using UnityEngine;
using Abstraction;
using UnitSystem;
using UnitSystem.Enum;
using BattleSystem.MainTower;

namespace BattleSystem
{
    public class TargetFinder
    {

        private MainTowerController _warBuildingsContainer;
        private IUnitsContainer _unitsContainer;
        private readonly IAttackTarget _defaultTarget;
        
        
        public TargetFinder(MainTowerController container)
        {
            _warBuildingsContainer = container;
            _defaultTarget = new NoneTarget();
        }
        
        public TargetFinder(MainTowerController warBuildingsContainer, IUnitsContainer unitsContainer)
        {
            _warBuildingsContainer = warBuildingsContainer;
            _unitsContainer = unitsContainer;
            _defaultTarget = new NoneTarget();
        }

        public IAttackTarget GetMainTower()
        {
            return _warBuildingsContainer.GetMainTower();
        }

        public IAttackTarget GetTarget(IUnit unit)
        {
            IAttackTarget result = new NoneTarget();

            while (unit.Priority.GetNext())
            {
                var currentPriority = unit.Priority.Current;

                switch (currentPriority.Priority)
                {
                    default:
                    case UnitPriorityType.MainTower:
                        {
                            result = GetMainTower();
                            break;
                        }

                    case UnitPriorityType.ClosestFoe:
                        {
                            result = GetClosestFoe(unit);
                            break;
                        }
                    case UnitPriorityType.SpecificFoe:
                        {
                            result = GetClosestFoe(unit, currentPriority.Type);
                            break;
                        }
                }

                if (result is not NoneTarget)
                {
                    break;
                }
            }

            unit.Priority.Reset();

            return result;
        }

        private IAttackTarget GetClosestFoe(IUnit unit, UnitType targetType = UnitType.None)
        {
            IAttackTarget target = new NoneTarget();
            
            List<IUnit> foeUnitList = (unit.Stats.Faction == UnitFactionType.Enemy) ? 
                _unitsContainer.DefenderUnits : _unitsContainer.EnemyUnits;

            Vector3 unitPos = unit.GetPosition();
            float minDist = float.MaxValue;

            for (int i = 0; i < foeUnitList.Count; i++)
            {
                IUnit foeUnit = foeUnitList[i];

                if( (targetType != UnitType.None && foeUnit.Stats.Type != targetType) || foeUnit is NoneTarget )
                    continue;

                Vector3 defenderPos = foeUnit.GetPosition();

                float distance = Vector3.Distance(unitPos, defenderPos);
                if (distance < minDist)
                {
                    minDist = distance;

                    target = foeUnit.View;
                }
            }

            return target;
        }

    }
}
