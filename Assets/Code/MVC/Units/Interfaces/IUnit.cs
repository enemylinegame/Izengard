﻿using Izengard.Abstraction.Interfaces;
using Izengard.Tools;

namespace Izengard.Units
{
    public interface IUnit : IDamageable<UnitDamage>, IDamageDealer<UnitDamage>
    {
        ObjectTransformModel TransformModel { get; }

        UnitFactionType Faction { get; }
        UnitType Type { get; }

        IParametr<int> Health { get; }
        IParametr<int> Armor { get; }
        IParametr<float> Size { get; }
        IParametr<float> Speed { get; }
        IParametr<float> DetectionRange { get; }

        IUnitDefence Defence { get; }
        IUnitOffence Offence { get; }

    }
}
