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
        private MainTowerController _mainTower;
        private IUnitsContainer _unitsContainer;
        private readonly IAttackTarget _defaultTarget;

        private bool _isMainTowerDestroyed;

        public TargetFinder(MainTowerController mainTower, IUnitsContainer unitsContainer)
        {
            _mainTower = mainTower;
            _unitsContainer = unitsContainer;

            _mainTower.OnMainTowerDestroyed += MainToweDestroyed;

            _defaultTarget = new NoneTarget();
        }

        private void MainToweDestroyed() => _isMainTowerDestroyed = false;

        public IAttackTarget GetMainTower()
        {
            if (_isMainTowerDestroyed)
                return _defaultTarget;

            return _mainTower.GetMainTower();
        }

        public IAttackTarget GetTarget(IUnit unit)
        {
            IAttackTarget result = _defaultTarget;

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

            List<IUnit> foeUnitList = (unit.Stats.Faction == FactionType.Enemy) ?
                _unitsContainer.DefenderUnits : _unitsContainer.EnemyUnits;

            Vector3 unitPos = unit.GetPosition();
            float minDist = float.MaxValue;

            for (int i = 0; i < foeUnitList.Count; i++)
            {
                IUnit foeUnit = foeUnitList[i];

                if(targetType != UnitType.None && foeUnit.Stats.Type != targetType)
                    continue;

                Vector3 foePos = foeUnit.GetPosition();

                float distance = Vector3.Distance(unitPos, foePos);
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
