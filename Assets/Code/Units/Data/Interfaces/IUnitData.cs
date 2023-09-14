namespace Units.Data
{
    public interface IUnitData
    {
        UnitFactionType Faction { get; }

        int HealthPoints { get; }
        int ArmorPoints { get; }

        float Size { get; }
        float Speed { get; }
        float DetectionRange { get; }

        IUnitDefenceData DefenceData { get; }
        IUnitOffenceData OffenceData { get; }
    }
}
