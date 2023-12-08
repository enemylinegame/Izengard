using Abstraction;

namespace UnitSystem
{
    public interface IUnitDefenceData
    {
        float EvadeChance { get; }
        float ArmorPoints { get; }
        IShieldData ShieldData { get; }
        IResistanceData ResistData { get; }
    }
}
