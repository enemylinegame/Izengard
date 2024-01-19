using Abstraction;
using System;
using UnitSystem.Enum;
using UnitSystem.Model;
using UnityEngine;

namespace UnitSystem
{
    public class UnitHandler : IUnit, IDisposable
    {
        private readonly IUnitView _view;
        private readonly UnitStatsModel _unitStats;       
        private readonly IUnitDefence _unitDefence;
        private readonly IUnitOffence _unitOffence;
        private readonly UnitNavigationModel _navigation;
        private readonly UnitStateModel _unitState;
        private readonly UnitTargetModel _unitTarget;
        private readonly UnitPriorityModel _priority;

        private string _id;
        private string _name;
        private Vector3 _startPosition;

        public IUnitView View => _view;

        public UnitStatsModel Stats => _unitStats;

        public IUnitDefence Defence => _unitDefence;

        public IUnitOffence Offence => _unitOffence;

        public UnitStateModel State => _unitState;
        public UnitTargetModel Target => _unitTarget;
        public UnitPriorityModel Priority => _priority;

        public string Id => _id;

        public string Name => _name;

        public Vector3 StartPosition => _startPosition;
        
        public float TimeProgress { get; set; }
      
        public event Action<IUnit> OnReachedZeroHealth;

        public UnitHandler(string name, IUnitView view, IUnitData unitData)
        {
            _name = name;

            _view = view;

            _unitStats = new UnitStatsModel(unitData); 

            _unitDefence = new UnitDefenceModel(unitData);

            _unitOffence = new UnitOffenceModel(unitData);

            _navigation 
                = new UnitNavigationModel(view.UnitNavigation, view.SelfTransform.position); ;

            _priority 
                = new UnitPriorityModel(unitData.UnitPriorities); ;

            _unitState = new UnitStateModel();
            
            _unitTarget = new UnitTargetModel();

            _id = _view.Id;
        }

        public void Enable()
        {
            Subscribe();

            _view.Show();
            _view.SetCollisionEnabled(true);

            _view.ChangeHealth(_unitStats.Health.GetValue());
            _view.ChangeSize(_unitStats.Size.GetValue());
            _view.ChangeSpeed(_unitStats.Speed.GetValue());

            _navigation.Enable();
            _unitState.ChangeState(UnitStateType.Idle);
        }

        private void Subscribe()
        {
            _unitStats.Health.OnValueChange += _view.ChangeHealth;
            _unitStats.Health.OnMinValueSet += ReachedZeroHealth;

            _unitStats.Size.OnValueChange += _view.ChangeSize;
            _unitStats.Speed.OnValueChange += _view.ChangeSpeed;

            _view.OnTakeDamage += TakeDamage;
        }

        public void Disable()
        {
            Unsubscribe();

            TimeProgress = 0;

            _unitTarget.ResetTarget();
            _navigation.Disable();
            _unitState.ChangeState(UnitStateType.None);

            _view.Hide();
        }

        private void Unsubscribe()
        {
            _unitStats.Health.OnValueChange -= _view.ChangeHealth;
            _unitStats.Health.OnMinValueSet -= ReachedZeroHealth;

            _unitStats.Size.OnValueChange -= _view.ChangeSize;
            _unitStats.Speed.OnValueChange -= _view.ChangeSpeed;
            
            _view.OnTakeDamage -= TakeDamage;
        }

        public void SetStartPosition(Vector3 spawnPosition)
        {
            _startPosition = spawnPosition;
            SetPosition(spawnPosition);
        }

        public void TakeDamage(IDamage damageValue)
        {
            var resultDamageAmount
                = _unitDefence.GetAfterDefDamage(damageValue);

            var hpLeft = _unitStats.Health.GetValue() - (int)resultDamageAmount;
            _unitStats.Health.SetValue(hpLeft);
        }

        public void ChangeState(UnitStateType state)
        {
            State.ChangeState(state);
            var animView = View.UnitAnimation;
            if (animView != null)
            {
                switch (state)
                {
                    case UnitStateType.None:
                        {
                            animView.Reset();
                            break;
                        }
                    case UnitStateType.Idle:
                        {
                            animView.IsMoving = false;
                            break;
                        }
                    case UnitStateType.Move:
                        {
                            animView.IsMoving = true;
                            break;
                        }
                    case UnitStateType.Attack:
                        {
                            animView.IsMoving = false;
                            break;
                        }
                    case UnitStateType.Die:
                        {
                            animView.StartDead();
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }


        private void ReachedZeroHealth(int value)
        {
            Target.ResetTarget();
            TimeProgress = 0.0f;
            _navigation.Disable();

            _view.SetCollisionEnabled(false);

            ChangeState(UnitStateType.Die);
            
            OnReachedZeroHealth?.Invoke(this);
        }

        #region IDamageDealer

        private IDamageable _damageableTarget;

        public IDamage GetAttackDamage()
        {
            return _unitOffence.GetDamage();
        }
        
        public void StartAttack(IDamageable damageableTarget)
        {
            _damageableTarget = damageableTarget;
        }

        #endregion

        #region IMovable

        public void MoveTo(Vector3 position)
        {
            //_navigation.Reset();
            _navigation.MoveTo(position);
        }

        public void Stop()
        {
            _navigation.Stop();
        }

        #endregion

        #region IPositioned

        public Vector3 GetPosition()
        {
            return _view.Position;
        }

        public void SetPosition(Vector3 pos)
        {
            _view.SelfTransform.position = pos;
        }

        #endregion

        #region IRotated

        public Vector3 GetRotation()
        {
            var angleVector = _view.SelfTransform.rotation.eulerAngles;
            return angleVector;
        }

        public void SetRotation(Vector3 rotation)
        {
            var newRotation = Quaternion.LookRotation(rotation);
            _view.SelfTransform.rotation = newRotation;
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
