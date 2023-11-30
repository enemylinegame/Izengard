using Abstraction;
using System;
using System.Collections.Generic;

namespace UnitSystem
{
    public interface IUnitsContainer
    {
        event Action<ITarget> OnUnitDead;

        event Action OnEnemyAdded;
        event Action OnDefenderAdded;

        event Action<IUnit> OnUnitRemoved;
        event Action OnAllEnemyDestroyed;
        event Action OnAllDefenderDestroyed;

        List<IUnit> EnemyUnits { get; }
        List<IUnit> DefenderUnits { get; }
        
        void AddUnit(IUnit unit);
    }
}