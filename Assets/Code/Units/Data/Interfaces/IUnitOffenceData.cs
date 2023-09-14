namespace Izengard.Units.Data
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
