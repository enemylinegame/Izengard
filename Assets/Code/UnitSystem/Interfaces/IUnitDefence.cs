using Abstraction;

namespace UnitSystem
{
    public interface IUnitDefence : IDefence<IUnitData, IDamage>
    {
        IParametr<float> ArmorPoints { get; }

        IParametr<float> BaseShieldPoints { get; }
        IParametr<float> FireShieldPoints { get; }
        IParametr<float> ColdShieldPoints { get; }
    }
}
