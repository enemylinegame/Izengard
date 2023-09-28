using System.Collections.Generic;
using UnityEngine;

namespace UnitSystem.Data
{
    [CreateAssetMenu(
        fileName = nameof(UnitSettings), 
        menuName = "UnitsData/" + nameof(UnitSettings))]
    public class UnitSettings : ScriptableObject, IUnitData
    {
        [SerializeField] private UnitMainStatsData _mainStats;
        [SerializeField] private List<UnitPriorityData> _unitPriorities;
        [SerializeField] private UnitDefenceData _defenceData;
        [SerializeField] private UnitOffenceData _offenceData;
        
        #region IUnitData

        public IUnitStatsData StatsData => _mainStats;

        public IReadOnlyList<UnitPriorityData> UnitPriorities => _unitPriorities;

        public IUnitDefenceData DefenceData => _defenceData; 
        public IUnitOffenceData OffenceData => _offenceData;

        #endregion
    }
}
