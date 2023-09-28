using UnitSystem.Enum;

namespace UnitSystem
{
    public interface IUnitOffenceData
    {
        UnitAttackType AttackType { get; }
        float MinRange { get; }
        float MaxRange { get; }
        float AttackSpeed { get; }
        float CriticalChance { get; }

        IUnitDamageData DamageData { get; }
    }
}
