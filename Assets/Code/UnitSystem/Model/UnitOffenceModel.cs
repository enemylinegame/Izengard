using Abstraction;
using UnitSystem.Enum;
using UnityEngine;

namespace UnitSystem.Model
{
    public class UnitOffenceModel : IUnitOffence
    {
        private readonly IUnitData _data;
        
        public UnitAttackType AttackType => _data.AttackType;

        public UnitAbilityType AbilityType => _data.AbilityType;

        public float MinRange => _data.MinRange;
        public float MaxRange => _data.MaxRange;

        public float CastingTime => _data.CastingTime;
        public float AttackTime => _data.AttackTime;

        public float LastAttackTime { get; set; }

        public UnitOffenceModel(IUnitData data)
        {
            _data = data;
            LastAttackTime = _data.AttackTime;
        }

        public IDamage GetDamage()
        {
            if (CheckChance(_data.FailChance))
            {
                return new DamageStructure
                {
                    BaseDamage = _data.OnFailDamage,
                    FireDamage = _data.OnFailDamage,
                    ColdDamage = _data.OnFailDamage
                };

            }

            if (CheckChance(_data.CriticalChance))
            {
                return new DamageStructure
                {
                    BaseDamage = _data.BaseDamage * _data.CritScale,
                    FireDamage = _data.FireDamage * _data.CritScale,
                    ColdDamage = _data.ColdDamage * _data.CritScale
                };
            }

            return new DamageStructure
            {
                BaseDamage = _data.BaseDamage,
                FireDamage = _data.FireDamage,
                ColdDamage = _data.ColdDamage
            };
        } 

        private bool CheckChance(float chanceValue)
        {
            var gainedChance = Random.value * 100f; 
            return gainedChance < chanceValue;
        }
    }
}
