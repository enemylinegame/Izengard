using System.Collections.Generic;
using UnitSystem.Data;
using UnitSystem.Enum;

namespace UnitSystem
{
    public interface IUnitData
    {
        public UnitFactionType Faction { get; }
        public UnitType Type { get; }
        public UnitRoleType Role { get; }
        public int HealthPoints { get; }
        public float Size { get; }
        public float Speed { get; }
        public float DetectionRange { get; }

        public IList<UnitPriorityData> UnitPriorities { get; }

        public float EvadeChance { get; }
        public float ArmorPoints { get; }
        public float BaseShieldPoints { get; }
        public float FireShieldPoints { get; }
        public float ColdShieldPoints { get; }
        public float BaseDamageResist { get; }
        public float FireDamageResist { get; }
        public float ColdDamageResist { get; }

        public UnitAttackType AttackType { get; }
        public UnitAbilityType AbilityType { get; }
        public float MinRange { get; }
        public float MaxRange { get; }
        public float CastingTime { get; }
        public float AttackTime { get; }
        public float CriticalChance { get; }
        public float CritScale { get; }
        public float FailChance { get; }
        public float OnFailDamage { get; }
        public float BaseDamage { get; }
        public float FireDamage { get; }
        public float ColdDamage { get; }
    }
}
