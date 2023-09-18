using Izengard.Abstraction.Interfaces;
using Izengard.UnitSystem.Enum;

namespace Izengard.UnitSystem
{
    public interface IUnit
    {
        UnitFactionType Faction { get; }
        UnitType Type { get; }

        IParametr<int> Health { get; }
        IParametr<float> Size { get; }
        IParametr<float> Speed { get; }
        IParametr<float> DetectionRange { get; }

        IUnitDefence Defence { get; }
        IUnitOffence Offence { get; }

    }
}
