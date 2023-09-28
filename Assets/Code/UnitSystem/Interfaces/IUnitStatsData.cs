using UnitSystem.Enum;

namespace UnitSystem
{
    public interface IUnitStatsData
    {
        UnitFactionType Faction { get; }
        UnitRoleType Role { get; }

        int HealthPoints { get; }
        float Size { get; }
        float Speed { get; }
        float DetectionRange { get; }
    }
}
