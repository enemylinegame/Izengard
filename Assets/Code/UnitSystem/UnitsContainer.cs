using Abstraction;
using System;
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

        public event Action<ITarget> OnUnitDead;

        public event Action OnAllEnemyDestroyed;

        public event Action OnAllDefenderDestroyed;

        public void AddUnit(IUnit unit)
        {
            if (unit == null) return;

            unit.Enable();
            unit.OnReachedZeroHealth += UnitReachedZeroHealth;

            switch (unit.Stats.Faction)
            {
                default:
                    break;
                case UnitFactionType.Enemy:
                    {
                        if (!_enemyUnits.Contains(unit))
                        {
                            _enemyUnits.Add(unit);
                        }
                        break;
                    }
                case UnitFactionType.Defender:
                    {
                        if (!_defenderUnits.Contains(unit))
                        {
                            _defenderUnits.Add(unit);
                        }
                        break;
                    }
            }
        }

        public void RemoveUnit(IUnit unit)
        {
            if (unit.State.Current == UnitStateType.Die)
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
                            unit.Target.ResetTarget();
                            unit.Disable();
                            _enemyUnits.Remove(unit);

                            if(_enemyUnits.Count == 0)
                            {
                                OnAllEnemyDestroyed?.Invoke();
                            }

                            break;
                        }
                    case UnitFactionType.Defender:
                        {
                            unit.Target.ResetTarget();
                            unit.Disable();
                            _defenderUnits.Remove(unit);

                            if (_defenderUnits.Count == 0)
                            {
                                OnAllDefenderDestroyed?.Invoke();
                            }

                            break;
                        }
                }
            }
        }

        private void UnitReachedZeroHealth(IUnit unit)
        {
            unit.OnReachedZeroHealth -= UnitReachedZeroHealth;

            unit.ChangeState(UnitStateType.Die);


            if (unit.Stats.Faction == UnitFactionType.Enemy)
            {
                _enemyUnits.Remove(unit);
            }
            else if (unit.Stats.Faction == UnitFactionType.Defender)
            {
                _defenderUnits.Remove(unit);
            }

            _deadUnits.Add(unit);

            OnUnitDead?.Invoke(unit.View);
        }
    }
}