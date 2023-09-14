namespace Izengard.Units.Data
{
    public interface IUnitDefenceData
    {
        float EvadeChance { get; }
        IUnitShieldData ShieldData { get; }
        IUnitResistanceData ResistData { get; }
    }
}
