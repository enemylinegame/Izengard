﻿using System.Collections.Generic;

namespace UnitSystem
{
    public interface IUnitsContainer
    {
        List<IUnit> EnemyUnits { get; }
        List<IUnit> DefenderUnits { get; }
        List<IUnit> DeadUnits { get; }

        void AddUnit(IUnit unit);
        void RemoveUnit(IUnit unit);
    }
}