using System.Collections.Generic;
using UnitSystem.Enum;

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

        public void AddUnit(IUnit unit)
        {
            if (unit == null) return;

            switch (unit.Stats.Faction)
            {
                default:
                    break;
                case UnitFactionType.Enemy:
                    {
                        if (!_enemyUnits.Contains(unit))
                        {
                            _enemyUnits.Add(unit);
                            unit.OnReachedZeroHealth += UnitReachedZeroHealth;
                            unit.Navigation.Enable();
                            unit.UnitState.ChangeState(UnitState.Idle);
                        }
                        break;
                    }
                case UnitFactionType.Defender:
                    {
                        if (!_defenderUnits.Contains(unit))
                        {
                            _defenderUnits.Add(unit);
                            unit.OnReachedZeroHealth += UnitReachedZeroHealth;
                            unit.Navigation.Enable();
                            unit.UnitState.ChangeState(UnitState.Idle);
                        }
                        break;
                    }
            }
        }

        public void RemoveUnit(IUnit unit)
        {
            if (unit.UnitState.CurrentState == UnitState.Die)
            {
                unit.Disable();
                _deadUnits.Remove(unit);
            }
            else
            {
                unit.OnReachedZeroHealth -= UnitReachedZeroHealth;

                switch (unit.Stats.Faction)
                {
                    default:
                        break;
                    case UnitFactionType.Enemy:
                        {
                            //Debug.Log($"FifthBattleController->RemoveUnit: [{unit.Id}]_{unit.Stats.Role} - dead");

                            unit.Target.ResetTarget();
                            unit.Disable();
                            _enemyUnits.Remove(unit);

                            break;
                        }
                    case UnitFactionType.Defender:
                        {
                            //Debug.Log($"FifthBattleController->RemoveUnit: {unit.View.SelfTransform.gameObject.name}");

                            unit.Target.ResetTarget();
                            unit.Disable();
                            _defenderUnits.Remove(unit);
                            break;
                        }
                }
            }
        }

        private void UnitReachedZeroHealth(IUnit unit)
        {
            unit.OnReachedZeroHealth -= UnitReachedZeroHealth;

            if (unit.Stats.Faction == UnitFactionType.Enemy)
            {
                _enemyUnits.Remove(unit);
            }
            else if (unit.Stats.Faction == UnitFactionType.Defender)
            {
                _defenderUnits.Remove(unit);
            }
            _deadUnits.Add(unit);
        }
    }
}