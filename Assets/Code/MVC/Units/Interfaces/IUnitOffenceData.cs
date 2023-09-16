namespace Izengard.Units
{
    public interface IUnitOffenceData
    {
        float AttackSpeed { get; }
        float MeleeAttackReach { get; }
        float RangedAttackMinRange { get; }
        float RangedAttackMaxRange { get; }
        float CriticalChance { get; }

        IUnitDamageData DamageData { get; }
    }
}
