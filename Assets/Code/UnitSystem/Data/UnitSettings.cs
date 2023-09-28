using UnitSystem.Enum;
using UnityEngine;

namespace UnitSystem.Data
{
    [CreateAssetMenu(fileName = nameof(UnitSettings), menuName = "UnitsData/" + nameof(UnitSettings))]
    public class UnitSettings : ScriptableObject, IUnitData
    {
        [SerializeField] private UnitFactionType _faction;
        [SerializeField] private UnitMainStatsData _mainStats;
        [SerializeField] private UnitDefenceSettings _defenceData;
        [SerializeField] private UnitOffenceSettings _offenceData;
        
        #region IUnitData

        public UnitFactionType Faction => _faction;
        public IUnitStatsData StatsData => _mainStats;
        public IUnitDefenceData DefenceData => _defenceData;

        public IUnitOffenceData OffenceData => _offenceData;
      
        #endregion
    }
}
