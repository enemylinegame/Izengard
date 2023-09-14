using UnityEngine;

namespace Izengard.Units.Data
{
    [CreateAssetMenu(fileName = nameof(UnitDefenceSettings), menuName = "UnitsData/" + nameof(UnitDefenceSettings))]
    public class UnitDefenceSettings : ScriptableObject, IUnitDefenceData
    {
        [Range(0,100)]
        [SerializeField] private float _evadeChance = 0;
        [SerializeField] private ShieldData _shieldData;
        [SerializeField] private ResistanceData _resistanceData;
       
        #region IUnitDefenceData

        public float EvadeChance => _evadeChance;

        public IUnitShieldData ShieldData => _shieldData;

        public IUnitResistanceData ResistData => _resistanceData;

        #endregion
    }
}
