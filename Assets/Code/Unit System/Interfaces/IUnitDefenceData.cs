namespace UnitSystem
{
    public interface IUnitDefenceData
    {
        float EvadeChance { get; }
        int ArmorPoints { get; }
        IUnitShieldData ShieldData { get; }
        IUnitResistanceData ResistData { get; }
    }
}
