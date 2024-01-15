using Abstraction;
using Configs;
using System;
using System.Collections.Generic;
using UI;
using UnitSystem.Enum;
using UserInputSystem;

namespace UnitSystem
{
    public class UnitsContainer : IUnitsContainer, IOnController, IOnUpdate, IPaused
    {
        private readonly float unitsDestroyDelay;
        private readonly UnitStatsPanel _unitStatsPanel;
        private readonly RayCastController _rayCastController;

        private List<IUnit> _createdUnits = new();

        private List<IUnit> _enemyUnits = new();
        private List<IUnit> _defenderUnits = new();

        private List<IUnit> toRemoveUnitsCollection = new();

        private IUnit _selectedUnit;

        public List<IUnit> EnemyUnits => _enemyUnits;
        public List<IUnit> DefenderUnits => _defenderUnits;

        public event Action<ITarget> OnUnitDead;
        
        public event Action<IUnit> OnUnitRemoved;

        public event Action OnEnemyAdded;
        public event Action OnDefenderAdded;

        public event Action OnAllEnemyDestroyed;

        public event Action OnAllDefenderDestroyed;
 
        public UnitsContainer(
            BattleSystemData data, 
            UnitStatsPanel unitStatsPanel,
            RayCastController rayCastController)
        {
            unitsDestroyDelay = data.UnitsDestroyDelay;
            _unitStatsPanel = unitStatsPanel;

            _rayCastController = rayCastController;

            _rayCastController.LeftClick += SelectUnit;
            _rayCastController.RightClick += RemoveSelection;
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

            _createdUnits.Add(unit);
        }

        private void UnitReachedZeroHealth(IUnit unit)
        {
            unit.OnReachedZeroHealth -= UnitReachedZeroHealth;

            switch (unit.Stats.Faction)
            {
                default:
                    break;
                case UnitFactionType.Enemy:
                    {
                        _enemyUnits.Remove(unit);

                        if (_enemyUnits.Count == 0)
                        {
                            OnAllEnemyDestroyed?.Invoke();
                        }

                        break;
                    }
                case UnitFactionType.Defender:
                    {
                        _defenderUnits.Remove(unit);

                        if (_defenderUnits.Count == 0)
                        {
                            OnAllDefenderDestroyed?.Invoke();
                        }

                        break;
                    }
            }

            OnUnitDead?.Invoke(unit.View);

            _createdUnits.Remove(unit);

            toRemoveUnitsCollection.Add(unit);
        }

        private void SelectUnit(string Id)
        {
            if (Id == null)
                return;

            if(_selectedUnit != null)
            {
                _unitStatsPanel.Dispose();
            }

            var unit = _createdUnits.Find(u => u.Id == Id);

            if (unit == null)
                return;

            _selectedUnit = unit;

            _unitStatsPanel.SetUnit(_selectedUnit);
        }

        public void RemoveSelection(string Id)
        {
            if (_selectedUnit != null)
            {
                _unitStatsPanel.Dispose();
            }

            _selectedUnit = null;
        }

        public void OnUpdate(float deltaTime)
        {
            if (IsPaused)
                return;

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

        public void ClearData()
        {
            if (_selectedUnit != null)
            {
                _unitStatsPanel.Dispose();
            }

            _selectedUnit = null;

            for (int i = 0; i < toRemoveUnitsCollection.Count; i++)
            {
                var unit = toRemoveUnitsCollection[i];

                unit.Disable();

                OnUnitRemoved?.Invoke(unit);
            }

            for (int i =0; i < _createdUnits.Count; i++)
            {
                var unit = _createdUnits[i];

                unit.OnReachedZeroHealth -= UnitReachedZeroHealth;

                unit.Disable();
                
                OnUnitRemoved?.Invoke(unit);
            }

            _createdUnits.Clear();
            _enemyUnits.Clear();
            _defenderUnits.Clear();
            toRemoveUnitsCollection.Clear();

        }

        #region IPaused

        public bool IsPaused { get; private set; }
        
        public void OnPause()
        {
            IsPaused = true;
        }

        public void OnRelease()
        {
            IsPaused = false;
        }

        #endregion
    }
}