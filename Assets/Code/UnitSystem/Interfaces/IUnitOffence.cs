using Abstraction;
using UnitSystem.Enum;

namespace UnitSystem
{
    public interface IUnitOffence : IOffence
    {
        UnitAttackType AttackType { get; }

        float MinRange { get; }
        float MaxRange { get; }

        float CastingTime { get; }
        float AttackTime { get; }

        float LastAttackTime { get; set; }
    }
}
