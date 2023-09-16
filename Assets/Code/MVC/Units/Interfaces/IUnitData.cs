namespace Izengard.Units
{
    public interface IUnitData
    {
        UnitFactionType Faction { get; }
        IUnitStatsData StatsData { get; }
        IUnitDefenceData DefenceData { get; }
        IUnitOffenceData OffenceData { get; }
    }
}
