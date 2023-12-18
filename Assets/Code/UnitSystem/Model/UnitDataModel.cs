using System.Collections.Generic;
using UnitSystem.Data;
using UnitSystem.Enum;

namespace UnitSystem.Model
{
    public class UnitDataModel
    {
        public UnitFactionType Faction { get; set; }
        public UnitType Type { get; set; }
        public UnitRoleType Role { get; set; }
        public int HealthPoints { get; set; }
        public float Size { get; set; }
        public float Speed { get; set; }
        public float DetectionRange { get; set; }

        public float EvadeChance { get; set; }
        public float ArmorPoints { get; set; }
        public float BaseShieldPoints { get; set; }
        public float FireShieldPoints { get; set; }
        public float ColdShieldPoints { get; set; }
        public float BaseDamageResist { get; set; }
        public float FireDamageResist { get; set; }
        public float ColdDamageResist { get; set; }

        public UnitAttackType AttackType { get; set; }
        public UnitAbilityType AbilityType { get; set; }
        public float MinRange { get; set; }
        public float MaxRange { get; set; }
        public float CastingTime { get; set; }
        public float AttackTime { get; set; }
        public float CriticalChance { get; set; }
        public float CritScale { get; set; }
        public float FailChance { get; set; }
        public float OnFailDamage { get; set; }
        public float BaseDamage { get; set; }
        public float FireDamage { get; set; }
        public float ColdDamage { get; set; }

        public IList<UnitPriorityData> UnitPriorities { get; set; }

    }
}
