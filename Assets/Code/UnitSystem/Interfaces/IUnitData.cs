using System.Collections.Generic;
using UnitSystem.Data;
using UnitSystem.Enum;

namespace UnitSystem
{
    public interface IUnitData
    {
        IUnitStatsData StatsData { get; }

        IReadOnlyList<UnitPriorityData> UnitPriorities { get; }
        
        IUnitDefenceData DefenceData { get; }
        IUnitOffenceData OffenceData { get; }
    }
}
