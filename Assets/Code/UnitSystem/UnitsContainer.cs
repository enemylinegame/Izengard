using Abstraction;
using Configs;
using System;
using System.Collections.Generic;
using Tools;
using UI;
using UserInputSystem;

namespace UnitSystem
{
    public class UnitsContainer : IUnitsContainer
    {
        private readonly float unitsDestroyDelay;
        private readonly UnitStatsPanel _unitStatsPanel;
        private readonly RayCastController _rayCastController;
        private readonly HealthBarManager _healthBarHandler;

        private readonly List<IUnit> _createdUnits = new();
        private readonly List<IUnit> _enemyUnits = new();
        private readonly List<IUnit> _defenderUnits = new();
        private readonly List<IUnit> _toRemoveUnitsCollection = new();

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

            _healthBarHandler = new HealthBarManager();

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
                case FactionType.Enemy:
                    {
                        if (!_enemyUnits.Contains(unit))
                        {
                            _enemyUnits.Add(unit);
                            OnEnemyAdded?.Invoke();
                        }
                        break;
                    }
                case FactionType.Defender:
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

            _healthBarHandler.Add(unit);
        }

        private void UnitReachedZeroHealth(IUnit unit)
        {
            unit.OnReachedZeroHealth -= UnitReachedZeroHealth;

            switch (unit.Stats.Faction)
            {
                case FactionType.Enemy:
                    {
                        _enemyUnits.Remove(unit);

                        if (_enemyUnits.Count == 0)
                        {
                            OnAllEnemyDestroyed?.Invoke();
                        }

                        break;
                    }
                case FactionType.Defender:
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

            _healthBarHandler.Remove(unit);
            
            _toRemoveUnitsCollection.Add(unit);
        }

        private void SelectUnit(string Id)
        {
            if (Id == null)
                return;

            UnselectUnit(_selectedUnit);

            var unit = _createdUnits.Find(u => u.Id == Id);

            if (unit == null)
                return;

            _selectedUnit = unit;

            _selectedUnit.View.Select();

            _unitStatsPanel.SetUnit(_selectedUnit);
        }

        private void RemoveSelection(string Id)
        {
             UnselectUnit(_selectedUnit);

             _selectedUnit = null;
        }

        private void UnselectUnit(IUnit selectedUnit)
        {
            if (selectedUnit != null)
            {
                _unitStatsPanel.Dispose();

                selectedUnit.View.Unselect();
            }
        }

        public void OnUpdate(float deltaTime)
        {
            if (IsPaused)
                return;

            UpdateToBeRemovedUnits(_toRemoveUnitsCollection, deltaTime);

            _healthBarHandler.OnUpdate(deltaTime);
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
                    
                    _toRemoveUnitsCollection.Remove(unit);
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

            for (int i = 0; i < _toRemoveUnitsCollection.Count; i++)
            {
                var unit = _toRemoveUnitsCollection[i];

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
            _toRemoveUnitsCollection.Clear();

        }

        #region IPaused

        public bool IsPaused { get; private set; }
        
        public void OnPause()
        {
            IsPaused = true;

            for(int i =0; i< _createdUnits.Count; i++)
            {
                var unit = _createdUnits[i];
                
                var unitAnim = unit.View.UnitAnimation;
                
                if(unitAnim != null)
                    unit.View.UnitAnimation.Stop();
            }
            for (int i = 0; i < _toRemoveUnitsCollection.Count; i++)
            {
                var unit = _toRemoveUnitsCollection[i];

                var unitAnim = unit.View.UnitAnimation;

                if (unitAnim != null)
                    unit.View.UnitAnimation.Stop();
            }
        }

        public void OnRelease()
        {
            IsPaused = false;

            for (int i = 0; i < _createdUnits.Count; i++)
            {
                var unit = _createdUnits[i];

                var unitAnim = unit.View.UnitAnimation;

                if (unitAnim != null)
                    unit.View.UnitAnimation.Play();
            }
            for (int i = 0; i < _toRemoveUnitsCollection.Count; i++)
            {
                var unit = _toRemoveUnitsCollection[i];

                var unitAnim = unit.View.UnitAnimation;

                if (unitAnim != null)
                    unit.View.UnitAnimation.Play();
            }
        }

        #endregion
    }
}