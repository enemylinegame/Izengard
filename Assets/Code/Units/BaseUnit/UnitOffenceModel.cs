using Izengard.Damage;
using Izengard.Units.Data;
using UnityEngine;

namespace Izengard.Units
{
    public class UnitOffenceModel
    {
        private const float CRIT_BASE_SCALE = 1f;
        private const float DAMAGE_SCALE_COEF = 2f;

        private readonly IUnitOffenceData _data;

        public UnitOffenceModel(IUnitOffenceData data)
        {
            _data = data;
        }

        public IUnitDamage GetDamage()
        {
            var critScale = GetCritScacle(_data.CriticalChance);

            var result = new UnitDamageModel
            {
                BaseDamage = _data.DamageData.BaseDamage * critScale,
                FireDamage = _data.DamageData.FireDamage * critScale,
                ColdDamage = _data.DamageData.ColdDamage * critScale
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
