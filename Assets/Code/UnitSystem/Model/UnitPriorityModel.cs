using System.Collections.Generic;
using UnitSystem.Data;
using UnitSystem.Enum;

namespace UnitSystem.Model
{
    public class UnitPriorityModel
    {
        private readonly List<UnitPriorityData> _unitPriorities;
        private int _priorityIndex;

        public bool IsEndPriority { get; private set; }

        public UnitPriorityModel(IReadOnlyList<UnitPriorityData> unitPriorities)
        {
            _unitPriorities = new List<UnitPriorityData>();

            foreach (var unitPriority in unitPriorities)
            {
                _unitPriorities.Add(unitPriority);
            }
        }

        public (UnitPriorityType priorityType, UnitRoleType roleType) GetNext() 
        {
            if(_unitPriorities.Count == 0)
            {
                return (UnitPriorityType.None, UnitRoleType.None);
            }

            _priorityIndex++;

            if (_priorityIndex > _unitPriorities.Count)
            {
                IsEndPriority = true;
                _priorityIndex = _unitPriorities.Count - 1;
            }

            var priorityData = _unitPriorities[_priorityIndex];

            return (priorityData.UnitPriority, priorityData.PriorityRole);
        }
    }
}
