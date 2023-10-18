using Abstraction;
using UnitSystem.Enum;
using UnityEngine;

namespace UnitSystem.Model
{
    public class UnitOffenceModel : IUnitOffence
    {
        private readonly IUnitOffenceData _offenceData;
        
        public UnitAttackType AttackType => _offenceData.AttackType;
        public float MinRange => _offenceData.MinRange;
        public float MaxRange => _offenceData.MaxRange;

        public float CastingSpeed => _offenceData.CastingTime;
        public float AttackSpeed => _offenceData.AttackTime;

        public UnitOffenceModel(IUnitOffenceData offenceData)
        {
            _offenceData = offenceData;
        }

        public IDamage GetDamage()
        {
            if (CheckChance(_offenceData.FailChance))
            {
                return new DamageStructure
                {
                    BaseDamage = _offenceData.OnFailDamage,
                    FireDamage = _offenceData.OnFailDamage,
                    ColdDamage = _offenceData.OnFailDamage
                };

            }

            var unitDamage = _offenceData.DamageData;

            if (CheckChance(_offenceData.CriticalChance))
            {
                return new DamageStructure
                {
                    BaseDamage = unitDamage.BaseDamage * _offenceData.CritScale,
                    FireDamage = unitDamage.FireDamage * _offenceData.CritScale,
                    ColdDamage = unitDamage.ColdDamage * _offenceData.CritScale
                };
            }

            return new DamageStructure
            {
                BaseDamage = unitDamage.BaseDamage,
                FireDamage = unitDamage.FireDamage,
                ColdDamage = unitDamage.ColdDamage
            };
        } 

        private bool CheckChance(float chanceValue)
        {
            var gainedChance = Random.Range(0, 101);
            return gainedChance <= chanceValue;
        }
    }
}
