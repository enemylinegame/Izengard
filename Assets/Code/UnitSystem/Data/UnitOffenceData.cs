using UnitSystem.Enum;
using UnityEngine;

namespace UnitSystem.Data
{
    [System.Serializable]
    public class UnitOffenceData : IUnitOffenceData
    {
        [SerializeField] private UnitAttackType _attackType;
        [SerializeField] private UnitAbilityType _abilityType;
        [SerializeField] private float _minRange = 0f;
        [SerializeField] private float _maxRange = 5f;
        [SerializeField] private float _castingTime = 1f;
        [SerializeField] private float _attackTime = 1f;
        
        [Space(5)]
        [Header("Chances Settings")]  
        [Range(0, 100)]
        [SerializeField] private float _criticalChance = 10f;
        [SerializeField] private float _critScale = 2f;
        [Range(0, 100)]
        [SerializeField] private float _failChance = 25f;
        [SerializeField] private float _onFailDamage = 1f;
        
        [Space(5)]
        [SerializeField] private UnitDamageData _damageData;

        #region IUnitOffenceData

        public UnitAttackType AttackType => _attackType;
        public UnitAbilityType AbilityType => _abilityType;

        public float MinRange => _minRange;

        public float MaxRange => _maxRange;

        public float CastingTime => _castingTime;

        public float AttackTime => _attackTime;

        public float CriticalChance => _criticalChance;

        public float CritScale => _critScale;

        public float FailChance => _failChance;

        public float OnFailDamage => _onFailDamage;

        public IUnitDamageData DamageData => _damageData;

        #endregion
    }
}
