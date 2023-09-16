namespace Izengard.Units
{
    public interface IUnitDefenceData
    {
        float EvadeChance { get; }
        IUnitShieldData ShieldData { get; }
        IUnitResistanceData ResistData { get; }
    }
}
