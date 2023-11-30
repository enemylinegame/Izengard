using System.Collections.Generic;
using UnitSystem.Data;
using UnitSystem.Enum;

namespace UnitSystem.Model
{
    public class UnitPriorityModel
    {
        private readonly List<UnitPriorityData> _priorities;
       
        private (UnitPriorityType Priority, UnitType Type) _currentPriority;

        private int _index = -1;
        public int Length => _priorities.Count;

        public (UnitPriorityType Priority, UnitType Type) Current => _currentPriority;

        public UnitPriorityModel(IReadOnlyList<UnitPriorityData> unitPriorities)
        {
            _priorities = new List<UnitPriorityData>();

            foreach (var unitPriority in unitPriorities)
            {
                _priorities.Add(unitPriority);
            }
        }

        public bool GetNext() 
        {
            if (_index == Length - 1)
                return false;

            _index++;

            var priorityData = _priorities[_index];
            _currentPriority = (priorityData.UnitPriority, priorityData.UnitType);

            return true;
        }

        public void Reset() => _index = -1;
    }
}
