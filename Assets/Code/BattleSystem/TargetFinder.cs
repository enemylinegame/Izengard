using Abstraction;
using BattleSystem.Buildings;
using BattleSystem.Buildings.Interfaces;
using UnitSystem;
using UnitSystem.Enum;
using UnityEngine;

namespace BattleSystem
{
    public class TargetFinder
    {
        private readonly LayerMask _buildingsMask;
        private readonly LayerMask _enemyMask;
        private readonly LayerMask _defenderMask;
        private readonly IAttackTarget _defaultTarget;

        private IWarBuildingsContainer _warBuildingsContainer;

        public TargetFinder(IWarBuildingsContainer container)
        {
            _warBuildingsContainer = container;

            _buildingsMask = LayerMask.GetMask("Building");
            _enemyMask =  LayerMask.GetMask("Enemy");
            _defenderMask = LayerMask.GetMask("Defender");

            _defaultTarget = new NoneTarget();
        }

        public IAttackTarget GetMainTower()
        {
            return _warBuildingsContainer.GetMainTowerAsAttackTarget();
        }

        public IAttackTarget GetClosestUnit(IUnit unit)
        {
            var findingUnit = _defaultTarget;

            var searchMask = GetUnitMask(unit.Stats.Faction);
            var unitPosition = unit.GetPosition();
            var searchRange = unit.Stats.DetectionRange.GetValue();
            
            var findedColliders = Physics.OverlapSphere(unitPosition, searchRange, searchMask);
            
            float maxDist = float.MaxValue;

            foreach (var collider  in findedColliders)
            {
                var findGO = collider.gameObject;

                if (findGO.TryGetComponent<IAttackTarget>(out var findedTarget))
                {
                    var distance
                        = Vector3.Distance(unitPosition, findedTarget.Position);

                    if (distance < maxDist)
                    {
                        maxDist = distance;
                        findingUnit = findedTarget;
                    }
                }
            }

            return findingUnit;
        }

        public IAttackTarget GetClosestUnit(IUnit unit, UnitRoleType findingRole)
        {
            var findingUnit = _defaultTarget;

            var searchMask = GetUnitMask(unit.Stats.Faction);
            var unitPosition = unit.GetPosition();
            var searchRange = unit.Stats.DetectionRange.GetValue();

            var findedColliders = Physics.OverlapSphere(unitPosition, searchRange, searchMask);

            float maxDist = float.MaxValue;

            foreach (var collider in findedColliders)
            {
                var findGO = collider.gameObject;
                
                if (findGO.tag != findingRole.ToString())
                    continue;

                if (findGO.TryGetComponent<IAttackTarget>(out var findedTarget))
                {
                    var distance
                        = Vector3.Distance(unitPosition, findedTarget.Position);

                    if (distance < maxDist)
                    {
                        maxDist = distance;
                        findingUnit = findedTarget;
                    }
                }
            }

            return findingUnit;
        }


        public IAttackTarget GetFarthestUnit(IUnit unit)
        {
            var findingUnit = _defaultTarget;

            var searchMask = GetUnitMask(unit.Stats.Faction);
            var unitPosition = unit.GetPosition();
            var searchRange = unit.Stats.DetectionRange.GetValue();

            var findedColliders = Physics.OverlapSphere(unitPosition, searchRange, searchMask);

            float minDist = 0;

            foreach (var collider in findedColliders)
            {
                var findGO = collider.gameObject;

                if (findGO.TryGetComponent<IAttackTarget>(out var findedTarget))
                {
                    var distance
                        = Vector3.Distance(unitPosition, findedTarget.Position);

                    if (distance > minDist)
                    {
                        minDist = distance;
                        findingUnit = findedTarget;
                    }
                }
            }

            return findingUnit;
        }

        private int GetUnitMask(UnitFactionType unitFaction)
        {
            if (unitFaction == UnitFactionType.Enemy)
                return _defenderMask;

            if (unitFaction == UnitFactionType.Defender)
                return _enemyMask;

            return 0;
        }
    }
}
