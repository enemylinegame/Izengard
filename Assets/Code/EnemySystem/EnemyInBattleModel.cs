using UnitSystem;

namespace EnemySystem
{
    public class EnemyInBattleModel
    {
        private IUnit _attackingUnit;
        private IUnit _attackedUnit;

        public IUnit AttackingUnit => _attackingUnit;
        public IUnit AttackedUnit => _attackedUnit;

        public EnemyInBattleModel(
            IUnit attackingUnit, 
            IUnit attackedUnit)
        {
            _attackingUnit = attackingUnit;
            _attackedUnit = attackedUnit;
        }

        public void ExecuteFight()
        {
            var enemyDamage = _attackingUnit.Offence.GetDamage();

            _attackedUnit.TakeDamage(enemyDamage);
        }

        public void RemoveFighters()
        {
            _attackingUnit = default;
            _attackedUnit = default;
        }
    }
}
