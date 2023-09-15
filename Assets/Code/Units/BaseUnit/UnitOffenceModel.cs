using Izengard.Damage;
using Izengard.Units.Data;
using UnityEngine;

namespace Izengard.Units
{
    public class UnitOffenceModel
    {
        private const float CRIT_BASE_SCALE = 1f;
        private const float DAMAGE_SCALE_COEF = 2f;

        private float _attackSpeed;
        private float _meleeAttackReach;
        private float _rangedAttackMinRange;
        private float _rangedAttackMaxRange;
       
        private float _criticalChance;
        private IUnitDamageData _damageData;

        public float AttackSpeed => _attackSpeed;
        public float MeleeAttackReach => _meleeAttackReach;
        public float RangedAttackMinRange => _rangedAttackMinRange;
        public float RangedAttackMaxRange => _rangedAttackMaxRange;

        public UnitOffenceModel(IUnitOffenceData data)
        {
            _attackSpeed = data.AttackSpeed;
            _meleeAttackReach = data.MeleeAttackReach;
            _rangedAttackMinRange = data.RangedAttackMinRange;
            _rangedAttackMaxRange = data.RangedAttackMaxRange;
            
            _criticalChance = data.CriticalChance;
            _damageData = data.DamageData;
        }

        public IUnitDamage GetDamage()
        {
            var critScale = GetCritScacle(_criticalChance);

            var result = new UnitDamageModel
            {
                BaseDamage = _damageData.BaseDamage * critScale,
                FireDamage = _damageData.FireDamage * critScale,
                ColdDamage = _damageData.ColdDamage * critScale
            };

            return result;
        } 

        private float GetCritScacle(float critChance)
        {
            var result = CRIT_BASE_SCALE;

            if (Random.Range(0, 101) >= critChance)
                result = DAMAGE_SCALE_COEF;

            return result;
        }
    }
}
