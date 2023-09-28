using Abstraction;

namespace UnitSystem
{
    public interface IUnitDefence : IDefence<IUnitDefenceData, IDamage>
    {
        IParametr<int> ArmorPoints { get; }

        IParametr<int> BaseShieldPoints { get; }
        IParametr<int> FireShieldPoints { get; }
        IParametr<int> ColdShieldPoints { get; }
    }
}
