namespace UnitSystem
{
    public interface IUnitDefenceData
    {
        float EvadeChance { get; }
        float ArmorPoints { get; }
        IUnitShieldData ShieldData { get; }
        IUnitResistanceData ResistData { get; }
    }
}
