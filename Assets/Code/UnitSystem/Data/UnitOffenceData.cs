using UnitSystem.Enum;
using UnityEngine;

namespace UnitSystem.Data
{
    [System.Serializable]
    public class UnitOffenceData : IUnitOffenceData
    {
        [SerializeField] private UnitAttackType _attackType;
        [SerializeField] private float _minRange = 0f;
        [SerializeField] private float _maxRange = 5f;
        [SerializeField] private float _attackSpeed = 1f;
        [Range(0, 100)]
        [SerializeField] private float _criticalChance = 10f;
        [SerializeField] private UnitDamageData _damageData;

        #region IUnitOffenceData

        public UnitAttackType AttackType => _attackType;

        public float MinRange => _minRange;

        public float MaxRange => _maxRange;

        public float AttackSpeed => _attackSpeed;

        public float CriticalChance => _criticalChance;

        public IUnitDamageData DamageData => _damageData;
     
        #endregion
    }
}
