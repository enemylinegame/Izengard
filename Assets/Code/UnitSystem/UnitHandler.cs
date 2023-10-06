using Abstraction;
using System;
using System.Collections.Generic;
using UnitSystem.Data;
using UnitSystem.Model;
using UnityEngine;

namespace UnitSystem
{
    public class UnitHandler : IUnit, IDisposable
    {
        private readonly IUnitView _unitView;
        private readonly UnitStatsModel _unitStats;       
        private readonly IUnitDefence _unitDefence;
        private readonly IUnitOffence _unitOffence;
        private readonly INavigation<Vector3> _navigation;
        private readonly UnitStateModel _unitState;
        private readonly UnitTargetModel _unitTarget;
        private readonly Queue<UnitPriorityData> _unitPriorities;

        private int _id;
        private Vector3 _spawnPosition;

        public IUnitView View => _unitView;

        public UnitStatsModel Stats => _unitStats;

        public IUnitDefence Defence => _unitDefence;

        public IUnitOffence Offence => _unitOffence;

        public INavigation<Vector3> Navigation => _navigation;

        public UnitTargetModel Target => _unitTarget;
        public UnitStateModel UnitState => _unitState;

        public Queue<UnitPriorityData> UnitPriorities => _unitPriorities;

        public int Id => _id;

        public Vector3 SpawnPosition => _spawnPosition;

      

        public event Action<IUnit> OnReachedZeroHealth;

        public UnitHandler(
            int index,
            IUnitView view, 
            UnitStatsModel unitStats,
            IUnitDefence unitDefence,
            IUnitOffence unitOffence,
            INavigation<Vector3> navigation,
            IReadOnlyList<UnitPriorityData> unitPriorities)
        {
            _id = index;

            _unitView = 
                view ?? throw new ArgumentNullException(nameof(view));

            _unitStats = 
                unitStats ?? throw new ArgumentNullException(nameof(unitStats));

            _unitDefence = 
                unitDefence ?? throw new ArgumentNullException(nameof(unitDefence));

            _unitOffence =
               unitOffence ?? throw new ArgumentNullException(nameof(unitOffence));

            _navigation = 
                navigation ?? throw new ArgumentNullException(nameof(navigation));

            _unitState = new UnitStateModel();
            
            _unitTarget = new UnitTargetModel();

            _unitPriorities = new Queue<UnitPriorityData>();
            
            foreach(var unitPriority in unitPriorities)
            {
                _unitPriorities.Enqueue(unitPriority);
            }

            _unitView.Hide();
        }

        public void Enable()
        {
            Subscribe();

            _unitView.Show();

            _unitView.ChangeHealth(_unitStats.Health.GetValue());
            _unitView.ChangeSize(_unitStats.Size.GetValue());
            _unitView.ChangeSpeed(_unitStats.Speed.GetValue());
        }

        private void Subscribe()
        {
            _unitStats.Health.OnValueChange += _unitView.ChangeHealth;
            _unitStats.Health.OnMinValueSet += ReachedZeroHealth;

            _unitStats.Size.OnValueChange += _unitView.ChangeSize;
            _unitStats.Speed.OnValueChange += _unitView.ChangeSpeed;
        }

        public void Disable()
        {
            Unsubscribe();

            _unitView.Hide();

            _navigation.Disable();
        }

        private void Unsubscribe()
        {
            _unitStats.Health.OnValueChange -= _unitView.ChangeHealth;
            _unitStats.Health.OnMinValueSet -= ReachedZeroHealth;

            _unitStats.Size.OnValueChange -= _unitView.ChangeSize;
            _unitStats.Speed.OnValueChange -= _unitView.ChangeSpeed;
        }

        public void SetSpawnPosition(Vector3 spawnPosition)
        {
            _spawnPosition = spawnPosition;
            SetPosition(spawnPosition);
        }

        private void ReachedZeroHealth(int value)
        {
            OnReachedZeroHealth?.Invoke(this);
        }

        #region IDamageable

        public void TakeDamage(IDamage damageValue)
        {
            var resultDamageAmount
                = _unitDefence.GetAfterDefDamage(damageValue);

            var hpLost = _unitStats.Health.GetValue() - resultDamageAmount;
            _unitStats.Health.SetValue(hpLost);
        }

        #endregion

        #region IDamageDealer

        public IDamage GetAttackDamage()
        {
            return _unitOffence.GetDamage();
        }

        #endregion

        #region IPositioned

        public Vector3 GetPosition()
        {
            return _unitView.SelfTransform.position;
        }

        public void SetPosition(Vector3 pos)
        {
            _unitView.SelfTransform.position = pos;
        }

        #endregion

        #region IRotated

        public Vector3 GetRotation()
        {
            var angleVector = _unitView.SelfTransform.rotation.eulerAngles;
            return angleVector;
        }

        public void SetRotation(Vector3 rotation)
        {
            var newRotation = Quaternion.Euler(rotation);
            _unitView.SelfTransform.rotation = newRotation;
        }

        #endregion

        #region IDisposable

        private bool _disposed = false;

        public void Dispose()
        {
            if (_disposed)
                return;

            Disable();

            _disposed = true;
        }

        #endregion
    }
}
