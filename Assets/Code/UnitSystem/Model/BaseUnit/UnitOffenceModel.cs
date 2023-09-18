using UnityEngine;

namespace Izengard.UnitSystem
{
    public class UnitOffenceModel : IUnitOffence
    {
        private const float CRIT_BASE_SCALE = 1f;
        private const float DAMAGE_SCALE_COEF = 2f;

        private readonly IUnitOffenceData _offenceData;

        public IUnitOffenceData OffenceData => _offenceData;

        public UnitOffenceModel(IUnitOffenceData offenceData)
        {
            _offenceData = offenceData;
        }

        public UnitDamage GetDamage()
        {
            var critScale = GetCritScacle(_offenceData.CriticalChance);

            var result = new UnitDamage
            {
                BaseDamage = _offenceData.DamageData.BaseDamage * critScale,
                FireDamage = _offenceData.DamageData.FireDamage * critScale,
                ColdDamage = _offenceData.DamageData.ColdDamage * critScale
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
