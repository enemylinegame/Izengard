using Izengard.Damage;
using Izengard.Tools;

namespace Izengard.Units
{
    public interface IUnit : IDamageable<IUnitDamage>, IDamageDealer<IUnitDamage>
    {
        ObjectTransformModel TransformModel { get; }

        UnitFactionType Faction { get; }
        UnitType Type { get; }

        ParametrModel<int> Health { get; }
        ParametrModel<int> Armor { get; }
        ParametrModel<float> Size { get; }
        ParametrModel<float> Speed { get; }
        ParametrModel<float> DetectionRange { get; }
    }
}
