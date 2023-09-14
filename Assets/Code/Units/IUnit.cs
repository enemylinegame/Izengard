using Izengard.Damage;
using Izengard.Units.Data;

namespace Izengard.Units
{
    public interface IUnit : IDamageable<IUnitDamage>, IDamageDealer<IUnitDamage>
    {
        UnitFactionType Faction { get; }
        int CurrentHealth { get; }
        int CurrentArmor { get; }

        void IncreaseHealth(int amount);
        void DecreaseHealth(int amount);
    }
}
