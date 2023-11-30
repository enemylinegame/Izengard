using UnitSystem.Enum;

namespace UnitSystem
{
    public interface IUnitOffenceData
    {
        UnitAttackType AttackType { get; }
        UnitAbilityType AbilityType { get; }
        float MinRange { get; }
        float MaxRange { get; }

        float CastingTime { get; }
        float AttackTime { get; }
        
        float CriticalChance { get; }
        float CritScale { get; }
        float FailChance { get; }
        float OnFailDamage { get; }

        IUnitDamageData DamageData { get; }
    }
}
