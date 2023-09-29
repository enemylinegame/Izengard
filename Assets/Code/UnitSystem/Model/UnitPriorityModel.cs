using System.Collections.Generic;
using UnitSystem.Data;
using UnitSystem.Enum;
using UnityEngine;

namespace UnitSystem
{
    public class UnitPriorityModel
    {
        private const int MAX_SEARCHES = 2;
        private const int CAST_RADIUS = 100;
        private const int BUILDING_MASK = 1 << 7;
        
        private readonly List<UnitPriorityData> _unitBasePriorities = 
            new List<UnitPriorityData>();

        private readonly Transform _unitTransorm;

        private Queue<UnitPriorityData> _unitQueuePriorities;

        private int _searchIndex;

        private Transform _mainTowerTransform;
        
        public Vector3 CurrentTargetPosition { get; private set; }

        public UnitPriorityModel(
            Transform unitTransform,
            IReadOnlyList<UnitPriorityData> unitPriorities)
        {
            _unitTransorm = unitTransform;          
         
            foreach (var priorityData in unitPriorities)
            {
                _unitBasePriorities.Add(priorityData);
            }

            _unitQueuePriorities = new Queue<UnitPriorityData>();

            InitQueue(_unitBasePriorities);

            _searchIndex = 0;
        }

        private void InitQueue(IReadOnlyList<UnitPriorityData> unitPriorities)
        {
            _unitQueuePriorities.Clear();

            foreach (var priorityData in unitPriorities)
            {
                _unitQueuePriorities.Enqueue(priorityData);
            }
        }

        public bool SetNextTarget()
        {
            if (_searchIndex >= MAX_SEARCHES)
                return false;

            if (_unitQueuePriorities.TryDequeue(out var nextPrioity))
            {
                switch (nextPrioity.UnitPriority)
                {
                    default:
                        break;

                    case UnitPriorityType.MainTower:
                        {
                            FindMainTower();
                            break;
                        }
                    case UnitPriorityType.ClosestFoe: 
                        {
                            Debug.Log("Try find ClosestFoe");
                            break;
                        }
                    case UnitPriorityType.FarthestFoe:
                        {
                            Debug.Log("Try find FarthestFoe");
                            break;
                        }
                    case UnitPriorityType.SpecificFoe:
                        {
                            FindSpecificFoe(nextPrioity.PriorityRole);
                            break;
                        }
                }

                return true;
            }



            return false;
        }

        private void FindSpecificFoe(UnitRoleType priorityRole)
        {
            Debug.Log($"Try find SpecificFoe - {priorityRole}");
        }

        private void FindMainTower()
        {
            if (_mainTowerTransform != null) 
            {
                CurrentTargetPosition = _mainTowerTransform.position;
                return;
            }

            var findResults = Physics.OverlapSphere(_unitTransorm.position, CAST_RADIUS, BUILDING_MASK);
            
            for(int i = 0; i < findResults.Length; i++)
            {
                if(findResults[i].gameObject.tag == "Player")
                {
                    _mainTowerTransform = findResults[i].gameObject.transform;
                    CurrentTargetPosition = _mainTowerTransform.position;
                    _searchIndex = 0;
                    return;
                }
            }

            _searchIndex++;
            SetNextTarget();
        }
    }
}
