using UnitSystem;
using UnitSystem.Enum;
using UnitSystem.View;
using UnityEngine;

namespace BattleSystem
{
    public class TargetFinder
    {
        private const int MAX_CAST_DISTANCE = 100;

        private readonly LayerMask _buildingsMask;
        private readonly LayerMask _enemyMask;
        private readonly LayerMask _defenderMask;
        private readonly BaseUnitView _defaultUnitView;

        private Transform _mainTowerTransform;

        public TargetFinder(Transform mainTowerTransform)
        {
            _mainTowerTransform = mainTowerTransform;

            _buildingsMask = LayerMask.GetMask("Building");
            _enemyMask =  LayerMask.GetMask("Enemy");
            _defenderMask = LayerMask.GetMask("Defender");

            var go = new GameObject("StubUnit");
            go.SetActive(false);
            _defaultUnitView = go.AddComponent<StubUnitView>();
        }

        public Vector3 GetMainTowerPosition()
        {
            return _mainTowerTransform.position;
        }

        public BaseUnitView GetClosestUnit(IUnit unit)
        {
            var findingUnit = _defaultUnitView;

            var searchMask = GetUnitMask(unit.Stats.Faction);
            var unitPosition = unit.GetPosition();
            var searchRange = unit.Stats.DetectionRange.GetValue();
            
            var findedColliders = Physics.OverlapSphere(unitPosition, searchRange, searchMask);
            
            float maxDist = float.MaxValue;

            foreach (var collider  in findedColliders)
            {
                var findGO = collider.gameObject;

                if (findGO.TryGetComponent<BaseUnitView>(out var findedFoe))
                {
                    var distance
                        = Vector3.Distance(unitPosition, findedFoe.SelfTransform.position);

                    if (distance < maxDist)
                    {
                        maxDist = distance;
                        findingUnit = findedFoe;
                    }
                }
            }

            return findingUnit;
        }

        public BaseUnitView GetClosestUnit(IUnit unit, UnitRoleType findingRole)
        {
            var findingUnit = _defaultUnitView;

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

                if (findGO.TryGetComponent<BaseUnitView>(out var findedFoe))
                {
                    var distance
                        = Vector3.Distance(unitPosition, findedFoe.SelfTransform.position);

                    if (distance < maxDist)
                    {
                        maxDist = distance;
                        findingUnit = findedFoe;
                    }
                }
            }

            return findingUnit;
        }


        public BaseUnitView GetFarthestUnit(IUnit unit)
        {
            var findingUnit = _defaultUnitView;

            var searchMask = GetUnitMask(unit.Stats.Faction);
            var unitPosition = unit.GetPosition();
            var searchRange = unit.Stats.DetectionRange.GetValue();

            var findedColliders = Physics.OverlapSphere(unitPosition, searchRange, searchMask);

            float minDist = 0;

            foreach (var collider in findedColliders)
            {
                var findGO = collider.gameObject;

                if (findGO.TryGetComponent<BaseUnitView>(out var findedFoe))
                {
                    var distance
                        = Vector3.Distance(unitPosition, findedFoe.SelfTransform.position);

                    if (distance > minDist)
                    {
                        minDist = distance;
                        findingUnit = findedFoe;
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
