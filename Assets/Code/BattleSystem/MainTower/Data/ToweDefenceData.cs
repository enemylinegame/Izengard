using Abstraction;
using UnityEngine;

namespace BattleSystem.MainTower
{
    [System.Serializable]
    public class ToweDefenceData
    {
        [SerializeField]
        [Min(0.0f)]
        private float _armorPoints = 20;

        [SerializeField] private ShieldData _shieldData;
        [SerializeField] private ResistanceData _resistanceData;

        public float ArmorPoints => _armorPoints;

        public IShieldData ShieldData => _shieldData;

        public IResistanceData ResistData => _resistanceData;
    }
}
