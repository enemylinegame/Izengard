using Izengard.Units.Data;

namespace Izengard.Units
{
    public interface IUnit : IDamageable<IUnitDamageData>
    {
        int CurrentHealth { get; }
        int CurrentArmor { get; }

        void IncreaseHealth(int amount);
        void DecreaseHealth(int amount);
    }
}
