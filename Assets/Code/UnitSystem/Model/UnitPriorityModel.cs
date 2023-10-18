using System.Collections.Generic;
using UnitSystem.Data;
using UnitSystem.Enum;

namespace UnitSystem.Model
{
    public class UnitPriorityModel
    {
        private readonly List<UnitPriorityData> _unitPriorities;
        private UnitPriorityType _currentPriority;

        private int _priorityIndex;

        public UnitPriorityType CurrentPriority => _currentPriority;
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

            if (_priorityIndex >= _unitPriorities.Count)
            {
                IsEndPriority = true;
                _priorityIndex = _unitPriorities.Count - 1;
            }

            var priorityData = _unitPriorities[_priorityIndex];
            _currentPriority = priorityData.UnitPriority;
            _priorityIndex++;

            return (priorityData.UnitPriority, priorityData.PriorityRole);
        }

        public void ResetIndex() => _priorityIndex = 0;
    }
}
