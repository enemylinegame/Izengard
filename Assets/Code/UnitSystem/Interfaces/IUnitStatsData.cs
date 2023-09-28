using UnitSystem.Enum;

namespace UnitSystem
{
    public interface IUnitStatsData
    {
        UnitType Type { get; }

        int HealthPoints { get; }
        float Size { get; }
        float Speed { get; }
        float DetectionRange { get; }
    }
}
