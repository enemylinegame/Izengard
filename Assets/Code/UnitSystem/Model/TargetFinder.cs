using UnitSystem.Enum;
using UnityEngine;

namespace UnitSystem
{
    public class TargetFinder
    {
        private const int CAST_RADIUS = 100;

        private const int BUILDING_MASK = 1 << 7;
        private const int ENEMY_MASK = 1 << 8;
        private const int DEFENDER_MASK = 1 << 9;

        private Transform _mainTowerTransform;

        public TargetFinder(Transform mainTowerTransform)
        {
            _mainTowerTransform = mainTowerTransform;          
  
        }

        public Vector3 GetMainTowerPosition()
        {
            return _mainTowerTransform.position;
        }

        public Vector3 GetClosestFoeLocation(IUnit unit)
        {
            var searchMask = GetUnitMask(unit.UnitStats.Faction);

            var unitPositin = unit.GetPosition();
            var resultPosition = unitPositin;

            var findResults = Physics.OverlapSphere(unitPositin, CAST_RADIUS, searchMask);
            float maxDist = float.MaxValue;
            
            for (int i = 0; i < findResults.Length; i++)
            {
                if(findResults[i].gameObject.TryGetComponent<IUnitView>(out var findedFoe))
                {
                    var targetPosition = findedFoe.SelfTransform.position;
                    var distance = Vector3.Distance(targetPosition, unitPositin);
                    if(distance < maxDist)
                    {
                        maxDist = distance;
                        resultPosition = targetPosition;
                    }
                }
            }

            return resultPosition;
        }

        public Vector3 GetFarthestFoeLocation(IUnit unit)
        {
            var searchMask = GetUnitMask(unit.UnitStats.Faction);

            var unitPositin = unit.GetPosition();
            var resultPosition = unitPositin;

            var findResults = Physics.OverlapSphere(unitPositin, CAST_RADIUS, searchMask);
            float minDist = 0;

            for (int i = 0; i < findResults.Length; i++)
            {
                if (findResults[i].gameObject.TryGetComponent<IUnitView>(out var findedFoe))
                {
                    var targetPosition = findedFoe.SelfTransform.position;
                    var distance = Vector3.Distance(targetPosition, unitPositin);
                    if (distance > minDist)
                    {
                        minDist = distance;
                        resultPosition = targetPosition;
                    }
                }
            }

            return resultPosition;
        }


       /* public Vector3 GetSpecificFoePosition(IUnit unit, UnitRoleType findingRole)
        {
            var searchMask = GetUnitMask(unit.UnitStats.Faction);

            var unitPositin = unit.GetPosition();
            var resultPosition = unitPositin;

            var findResults = Physics.OverlapSphere(unitPositin, CAST_RADIUS, searchMask);
            
            float maxDist = float.MaxValue;
            
            for (int i = 0; i < findResults.Length; i++)
            {
                if (findResults[i].gameObject.tag != findingRole.ToString())
                    continue;

                if (findResults[i].gameObject.TryGetComponent<IUnitView>(out var findedFoe))
                {
                    var targetPosition = findedFoe.SelfTransform.position;
                    var distance = Vector3.Distance(targetPosition, unitPositin);
                    if (distance < maxDist)
                    {
                        maxDist = distance;
                        resultPosition = targetPosition;
                    }
                }
            }

            return resultPosition;
        }*/


        private int GetUnitMask(UnitFactionType unitFaction)
        {
            if (unitFaction == UnitFactionType.Enemy)
                return DEFENDER_MASK;

            if (unitFaction == UnitFactionType.Defender)
                return ENEMY_MASK;

            return 0;
        }
    }
}
