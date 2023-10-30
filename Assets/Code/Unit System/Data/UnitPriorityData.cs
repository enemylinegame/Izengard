using UnitSystem.Enum;
using UnityEngine;

namespace UnitSystem.Data
{
    [System.Serializable]
    public class UnitPriorityData
    {
        [SerializeField]
        private UnitPriorityType _unitPriority = UnitPriorityType.None;

        [SerializeField]
        [DrawIf("_unitPriority", UnitPriorityType.SpecificFoe)] 
        private UnitRoleType _priorityRole = UnitRoleType.None;

        public UnitPriorityType UnitPriority => _unitPriority;
        public UnitRoleType PriorityRole => _priorityRole;
    }
}
