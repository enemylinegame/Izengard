using Abstraction;
using UnityEngine;

namespace UnitSystem.Data
{

    [System.Serializable]
    public class UnitDefenceData : IUnitDefenceData
    {
        [Range(0,100)]
        [SerializeField] 
        private float _evadeChance = 0;
        
        [SerializeField]
        [Min(0.0f)]
        private float _armorPoints = 20;

        [SerializeField] private ShieldData _shieldData;
        [SerializeField] private ResistanceData _resistanceData;
       
        #region IUnitDefenceData

        public float EvadeChance => _evadeChance;
        public float ArmorPoints => _armorPoints;

        public IShieldData ShieldData => _shieldData;

        public IResistanceData ResistData => _resistanceData;

        #endregion
    }
}
