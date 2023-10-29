using Abstraction;
using System;
using UnitSystem.Enum;
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
        private readonly UnitPriorityModel _priority;

        private int _id;
        private Vector3 _startPosition;

        public IUnitView View => _unitView;

        public UnitStatsModel Stats => _unitStats;

        public IUnitDefence Defence => _unitDefence;

        public IUnitOffence Offence => _unitOffence;

        public INavigation<Vector3> Navigation => _navigation;

        public UnitTargetModel Target => _unitTarget;
        public UnitStateModel UnitState => _unitState;
        public UnitPriorityModel Priority => _priority;

        public int Id => _id;

        public Vector3 StartPosition => _startPosition;

        public event Action<IUnit> OnReachedZeroHealth;

        public UnitHandler(
            int index,
            IUnitView view, 
            UnitStatsModel unitStats,
            IUnitDefence unitDefence,
            IUnitOffence unitOffence,
            INavigation<Vector3> navigation,
            UnitPriorityModel priority)
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

            _priority = 
                priority ?? throw new ArgumentNullException(nameof(priority));

            _unitState = new UnitStateModel();
            
            _unitTarget = new UnitTargetModel();

            _unitView.Init(_id);
        }

        public void Enable()
        {
            Subscribe();

            _unitView.Show();
            _unitView.SetCollisionEnabled(true);

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

            _unitState.OnStateChange += OnStateChanged;
        }

        public void Disable()
        {
            Unsubscribe();

            _navigation.Disable();
            
            _unitView.Hide();
        }

        private void Unsubscribe()
        {
            _unitStats.Health.OnValueChange -= _unitView.ChangeHealth;
            _unitStats.Health.OnMinValueSet -= ReachedZeroHealth;

            _unitStats.Size.OnValueChange -= _unitView.ChangeSize;
            _unitStats.Speed.OnValueChange -= _unitView.ChangeSpeed;
            
            _unitState.OnStateChange -= OnStateChanged;
        }

        public void SetStartPosition(Vector3 spawnPosition)
        {
            _startPosition = spawnPosition;
            SetPosition(spawnPosition);
        }

        private void ReachedZeroHealth(int value)
        {
            OnReachedZeroHealth?.Invoke(this);
        }

        private void OnStateChanged(UnitState newState)
        {
            if (newState == Enum.UnitState.Die)
            {
                _unitView.SetCollisionEnabled(false);
                _navigation.Disable();
            }
        }
        
        
        #region IDamageable

        public bool IsAlive => _unitStats.Health.GetValue() > 0;

        public void TakeDamage(IDamage damageValue)
        {
            var resultDamageAmount
                = _unitDefence.GetAfterDefDamage(damageValue);

            var hpLeft = _unitStats.Health.GetValue() - (int)resultDamageAmount;
            _unitStats.Health.SetValue(hpLeft);
        }

        #endregion

        #region IDamageDealer

        public event Action<IDamageDealer, IDamageable> OnAttackProcessEnd;

        private IDamageable _damageableTarget;

        public IDamage GetAttackDamage()
        {
            return _unitOffence.GetDamage();
        }
        
        public void StartAttack(IDamageable damageableTarget)
        {
            _damageableTarget = damageableTarget;
            OnAttackProcessEnd?.Invoke(this, _damageableTarget);
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
