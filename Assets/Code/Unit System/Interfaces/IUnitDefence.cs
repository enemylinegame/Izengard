using Izengard.Abstraction.Interfaces;

namespace Izengard.UnitSystem
{
    public interface IUnitDefence : IDefence<IUnitDefenceData, UnitDamage>
    {
        IParametr<int> BaseShieldPoints { get; }
        IParametr<int> FireShieldPoints { get; }
        IParametr<int> ColdShieldPoints { get; }
    }
}
