using UnitSystem;
using UnitSystem.Enum;
using UnitSystem.View;
using UnityEngine;

namespace BattleSystem
{
    public class TargetFinder
    {
        private const int CAST_RADIUS = 100;

        private readonly int _buildingsMask;
        private readonly int _enemyMask;
        private readonly int _defenderMask;

        private Transform _mainTowerTransform;

        public TargetFinder(Transform mainTowerTransform)
        {
            _mainTowerTransform = mainTowerTransform;

            _buildingsMask = LayerMask.GetMask("Building");
            _enemyMask = LayerMask.GetMask("Enemy");
            _defenderMask = LayerMask.GetMask("Defender");
        }

        public Vector3 GetMainTowerPosition()
        {
            return _mainTowerTransform.position;
        }

        public BaseUnitView GetClosestUnit(IUnit unit)
        {
            var searchMask = GetUnitMask(unit.UnitStats.Faction);

            var unitPositin = unit.GetPosition();

            BaseUnitView findingUnit = new StubUnitView();

            var findResults = Physics.OverlapSphere(unitPositin, CAST_RADIUS, searchMask);
            float maxDist = float.MaxValue;

            for (int i = 0; i < findResults.Length; i++)
            {
                if (findResults[i].gameObject.TryGetComponent<BaseUnitView>(out var findedFoe))
                {
                    var targetPosition = findedFoe.SelfTransform.position;
                    var distance = Vector3.Distance(targetPosition, unitPositin);
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
            var searchMask = GetUnitMask(unit.UnitStats.Faction);

            var unitPositin = unit.GetPosition();
            BaseUnitView findingUnit = new StubUnitView();

            var findResults = Physics.OverlapSphere(unitPositin, CAST_RADIUS, searchMask);

            float maxDist = float.MaxValue;

            for (int i = 0; i < findResults.Length; i++)
            {
                if (findResults[i].gameObject.tag != findingRole.ToString())
                    continue;

                if (findResults[i].gameObject.TryGetComponent<BaseUnitView>(out var findedFoe))
                {
                    var targetPosition = findedFoe.SelfTransform.position;
                    var distance = Vector3.Distance(targetPosition, unitPositin);
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
            var searchMask = GetUnitMask(unit.UnitStats.Faction);

            var unitPositin = unit.GetPosition();
            BaseUnitView findingUnit = new StubUnitView();

            var findResults = Physics.OverlapSphere(unitPositin, CAST_RADIUS, searchMask);
            float minDist = 0;

            for (int i = 0; i < findResults.Length; i++)
            {
                if (findResults[i].gameObject.TryGetComponent<BaseUnitView>(out var findedFoe))
                {
                    var targetPosition = findedFoe.SelfTransform.position;
                    var distance = Vector3.Distance(targetPosition, unitPositin);
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
