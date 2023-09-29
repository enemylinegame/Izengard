using UnitSystem.Enum;

namespace UnitSystem
{
    public interface IUnitOffenceData
    {
        UnitAttackType AttackType { get; }
        
        float MinRange { get; }
        float MaxRange { get; }

        float CastingSpeed { get; }
        float AttackSpeed { get; }
        
        float CriticalChance { get; }
        float CritScale { get; }
        float FailChance { get; }
        float OnFailDamage { get; }

        IUnitDamageData DamageData { get; }
    }
}
