using System.Collections.Generic;

namespace UnitSystem
{
    public class UnitsContainer : IUnitsContainer
    {
        
        private List<IUnit> _enemyUnits = new();
        private List<IUnit> _defenderUnits = new();
        private List<IUnit> _deadUnits = new();

        public List<IUnit> EnemyUnits => _enemyUnits;
        public List<IUnit> DefenderUnits => _defenderUnits;
        public List<IUnit> DeadUnits => _deadUnits;

    }
}