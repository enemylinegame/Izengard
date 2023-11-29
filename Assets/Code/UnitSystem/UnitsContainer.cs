using Abstraction;
using Configs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnitSystem.Enum;

namespace UnitSystem
{
    public class UnitsContainer : IUnitsContainer, IOnUpdate
    {
        private readonly float unitsDestroyDelay;

        private List<IUnit> _enemyUnits = new();
        private List<IUnit> _defenderUnits = new();

        private List<IUnit> toRemoveUnitsCollection = new();

        public List<IUnit> EnemyUnits => _enemyUnits;
        public List<IUnit> DefenderUnits => _defenderUnits;

        public event Action<ITarget> OnUnitDead;
        
        public event Action<IUnit> OnUnitRemoved;

        public event Action OnEnemyAdded;
        public event Action OnDefenderAdded;

        public event Action OnAllEnemyDestroyed;

        public event Action OnAllDefenderDestroyed;
 
        public UnitsContainer(BattleSystemData data)
        {
            unitsDestroyDelay = data.UnitsDestroyDelay;
        }

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
                            OnEnemyAdded?.Invoke();
                        }
                        break;
                    }
                case UnitFactionType.Defender:
                    {
                        if (!_defenderUnits.Contains(unit))
                        {
                            _defenderUnits.Add(unit);
                            OnDefenderAdded?.Invoke();
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
                toRemoveUnitsCollection.Remove(unit);       
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
                            _enemyUnits.Remove(unit);

                            if(_enemyUnits.Count == 0)
                            {
                                OnAllEnemyDestroyed?.Invoke();
                            }

                            break;
                        }
                    case UnitFactionType.Defender:
                        {
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

            OnUnitRemoved?.Invoke(unit);
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
            
            OnUnitDead?.Invoke(unit.View);

            toRemoveUnitsCollection.Add(unit);
        }

        public void OnUpdate(float deltaTime)
        {
            UpdateToBeRemovedUnits(toRemoveUnitsCollection, deltaTime);
        }

        private void UpdateToBeRemovedUnits(List<IUnit> toRemoveUnits, float deltaTime)
        {
            if (toRemoveUnits.Count == 0)
                return;

            for (int i = (toRemoveUnits.Count - 1); i >= 0; i--)
            {
                var unit = toRemoveUnits[i];
                
                unit.TimeProgress += deltaTime;
                
                if(unit.TimeProgress >= unitsDestroyDelay)
                {
                    unit.Disable();
                    OnUnitRemoved?.Invoke(unit);
                    
                    toRemoveUnitsCollection.Remove(unit);
                }
            }
        }
    }
}