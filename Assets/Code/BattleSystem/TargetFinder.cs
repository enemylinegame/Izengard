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
        private readonly StubUnitView _defaultUnitView;

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
            var searchMask = GetUnitMask(unit.Stats.Faction);

            var unitPosition = unit.GetPosition();

            BaseUnitView findingUnit = _defaultUnitView;

            var searchRange = unit.Stats.DetectionRange.GetValue();

            var findedHits = 
                Physics.SphereCastAll(
                    unitPosition,
                    searchRange, 
                    unit.View.SelfTransform.forward,
                    MAX_CAST_DISTANCE, 
                    searchMask);

            float maxDist = float.MaxValue;

            foreach(var hit in findedHits)
            {
                var findGO = hit.transform.gameObject;

                if (findGO.TryGetComponent<BaseUnitView>(out var findedFoe))
                {
                    var distance = hit.distance;

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
            var searchMask = GetUnitMask(unit.Stats.Faction);

            var unitPosition = unit.GetPosition();

            BaseUnitView findingUnit = _defaultUnitView;

            var searchRange = unit.Stats.DetectionRange.GetValue();
            var radius = searchRange / 2;

            var findedHits =
                Physics.SphereCastAll(
                    unitPosition,
                    searchRange,
                    unit.View.SelfTransform.forward,
                    MAX_CAST_DISTANCE,
                    searchMask);

            float maxDist = float.MaxValue;

            foreach (var hit in findedHits)
            {
                var findGO = hit.transform.gameObject;

                if (findGO.tag != findingRole.ToString())
                    continue;

                if (findGO.TryGetComponent<BaseUnitView>(out var findedFoe))
                {
                    var distance = hit.distance;

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
            var searchMask = GetUnitMask(unit.Stats.Faction);

            var unitPosition = unit.GetPosition();

            BaseUnitView findingUnit = _defaultUnitView;

            var searchRange = unit.Stats.DetectionRange.GetValue();

            var findedHits =
               Physics.SphereCastAll(
                   unitPosition,
                   searchRange,
                   unit.View.SelfTransform.forward,
                   MAX_CAST_DISTANCE,
                   searchMask);

            float minDist = 0;

            foreach (var hit in findedHits)
            {
                var findGO = hit.transform.gameObject;

                if (findGO.TryGetComponent<BaseUnitView>(out var findedFoe))
                {
                    var distance = hit.distance;

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
