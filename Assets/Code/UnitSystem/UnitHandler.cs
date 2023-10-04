using Abstraction;
using System;
using UnitSystem.Enum;
using UnityEngine;

namespace UnitSystem
{
    public class UnitHandler : IUnit, IDisposable
    {
        private readonly IUnitView _unitView;
        
        private readonly UnitStatsModel _unitStats;       
        private readonly IUnitDefence _unitDefence;
        private readonly IUnitOffence _unitOffence;
        private readonly UnitPriorityModel _unitPriority;
        private readonly INavigation<Vector3> _navigation;
        private readonly UnitStateModel _unitState;

        private int _id;
        private UnitState _state;

        public int Id => _id;
        public UnitState State => _state;

        public IUnitView UnitView => _unitView;

        public UnitStatsModel UnitStats => _unitStats;

        public IUnitDefence UnitDefence => _unitDefence;

        public IUnitOffence UnitOffence => _unitOffence;

        public UnitPriorityModel UnitPriority => _unitPriority;

        public INavigation<Vector3> Navigation => _navigation;

        public UnitStateModel UnitState => _unitState;

        public UnitHandler(
            int index,
            IUnitView view, 
            UnitStatsModel unitStats,
            IUnitDefence unitDefence,
            IUnitOffence unitOffence,
            UnitPriorityModel unitPriority,
            INavigation<Vector3> navigation)
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

            _unitPriority  =
                unitPriority ?? throw new ArgumentNullException(nameof(unitPriority));

            _navigation = 
                navigation ?? throw new ArgumentNullException(nameof(navigation));


            _unitState = new UnitStateModel();

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
            _unitStats.Size.OnValueChange -= _unitView.ChangeSize;
            _unitStats.Speed.OnValueChange -= _unitView.ChangeSpeed;
        }

        public void ChangeState(UnitState newState)
        {
            if(newState != _state)
            {
                _state = newState;
            }
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
