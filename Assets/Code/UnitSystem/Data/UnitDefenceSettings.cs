using UnityEngine;

namespace UnitSystem.Data
{

    [System.Serializable]
    public class UnitDefenceData : IUnitDefenceData
    {
        [Range(0,100)]
        [SerializeField] private float _evadeChance = 0;
        [SerializeField] private int _armorPoints = 20;

        [SerializeField] private ShieldData _shieldData;
        [SerializeField] private ResistanceData _resistanceData;
       
        #region IUnitDefenceData

        public float EvadeChance => _evadeChance;
        public int ArmorPoints => _armorPoints;

        public IUnitShieldData ShieldData => _shieldData;

        public IUnitResistanceData ResistData => _resistanceData;

        #endregion
    }
}
