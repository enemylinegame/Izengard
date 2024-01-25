using Abstraction;
using UnitSystem.Enum;

namespace UnitSystem
{
    public interface IUnitStatsData
    {
        FactionType Faction { get; }
        
        UnitType Type { get; }
        UnitRoleType Role { get; }

        int HealthPoints { get; }
        float Size { get; }
        float Speed { get; }
        float DetectionRange { get; }
    }
}
