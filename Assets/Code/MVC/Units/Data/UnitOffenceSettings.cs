using UnityEngine;

namespace Izengard.Units.Data
{
    [CreateAssetMenu(fileName = nameof(UnitOffenceSettings), menuName = "UnitsData/" + nameof(UnitOffenceSettings))]
    public class UnitOffenceSettings : ScriptableObject, IUnitOffenceData
    {
        [SerializeField] private float _attackSpeed = 1f;
        [SerializeField] private float _meleeAttackReach = 1f;
        [SerializeField] private float _rangedAttackMinRange = 5f;
        [SerializeField] private float _rangedAttackMaxRange = 10f;
        [Range(0, 100)]
        [SerializeField] private float _criticalChance = 10f;
        [SerializeField] private DamageData _damageData;

        #region IUnitOffenceData

        public float AttackSpeed => _attackSpeed;

        public float MeleeAttackReach => _meleeAttackReach;

        public float RangedAttackMinRange => _rangedAttackMinRange;

        public float RangedAttackMaxRange => _rangedAttackMaxRange;

        public float CriticalChance => _criticalChance;

        public IUnitDamageData DamageData => _damageData;

        #endregion
    }
}
