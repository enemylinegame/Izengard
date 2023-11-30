using Abstraction;

namespace UnitSystem
{
    public interface IUnitDefence : IDefence<IUnitDefenceData, IDamage>
    {
        IParametr<float> ArmorPoints { get; }

        IParametr<float> BaseShieldPoints { get; }
        IParametr<float> FireShieldPoints { get; }
        IParametr<float> ColdShieldPoints { get; }
    }
}
