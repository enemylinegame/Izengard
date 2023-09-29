using UnitSystem;
using UnitSystem.Enum;
using UnityEngine;

namespace EnemySystem
{
    public class EnemyController : IOnController, IOnUpdate, IOnFixedUpdate
    {
        private const int ENEMY_LAYER = 8;

        private readonly IUnit _unit;

        private Vector3 _currentTarget;
        private float _enemyStopDistance;

        private bool _isEnable;
        private bool _isMove;
        private bool _isReachTarget;

        public EnemyController(IUnit unit)
        {
            _unit = unit;

            if (_unit.Model.Offence.AttackType == UnitAttackType.Melee)
            {
                _enemyStopDistance = _unit.Model.Offence.MinRange;
            }
            else if (_unit.Model.Offence.AttackType == UnitAttackType.Range) 
            {
                _enemyStopDistance = _unit.Model.Offence.MaxRange;
            }

            _unit.View.OnPulledInFight += OnPullInFight;

            _isEnable = false;
            _isMove = false;
            _isReachTarget = false;
        }

        public void Enable()
        {
            _isEnable = true;

            _unit.Enable();

            _unit.UnitPriority.SetNextTarget();

            _currentTarget = _unit.UnitPriority.CurrentTargetPosition;

            _unit.Navigation.Enable();
            MoveToTarget(_currentTarget);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isEnable == false)
                return;

            if (_isReachTarget)
            {
                _isMove = false;

                if (IsGetIntoFight() == false)
                {
                    _currentTarget = _unit.UnitPriority.CurrentTargetPosition;

                    if (CheckStopDistance(_currentTarget) == true)
                    {
                        Debug.Log("Reached primary trget");
                        _isEnable = false;
                    }
                    else
                    {
                        MoveToTarget(_currentTarget);
                    }
                }
                else
                {
                    OnPullInFight();
                }

                var direction = (_currentTarget - _unit.GetPosition()).normalized;
                var lookRotation = Quaternion.LookRotation(direction);
                _unit.SetRotation(lookRotation.eulerAngles);

                _isReachTarget = false;
                return;
            }

            var target = CheckForUnitsInRange();
            if(target != _currentTarget)
            {
                _currentTarget = target;
                MoveToTarget(_currentTarget);
            }
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            if (_isEnable == false)
                return;

            if (_isMove)
            {
                if (CheckStopDistance(_currentTarget) == true)
                {
                    _unit.Navigation.Stop();
                    _isMove = false;
                    _isReachTarget = true;
                }
            }
        }
       
        private void MoveToTarget(Vector3 target)
        {
            _unit.Navigation.Stop();

            _unit.Navigation.MoveTo(target);
            _isMove = true;
        }
      
        private void OnPullInFight()
        {
            Debug.Log($"Enemy[{_unit.Model.Role}] - {_unit.Id} is in fight!");

            _isEnable = false;
        }

        private bool IsGetIntoFight()
        {
            var selfPos = _unit.GetPosition();
            var colliders = Physics.OverlapSphere(selfPos, _enemyStopDistance);
            if (colliders.Length != 0)
            {
                foreach (var collider in colliders)
                {
                    var go = collider.gameObject;
                    if (go.layer != ENEMY_LAYER && go.TryGetComponent<IUnitView>(out var unit))
                    {
                        if (unit.IsFighting == false) 
                        {
                            unit.PullIntoFight();
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private Vector3 CheckForUnitsInRange()
        {
            var selfPos = _unit.GetPosition();
            var range = Mathf.Max(_unit.Model.DetectionRange.GetValue(), _enemyStopDistance);
            var colliders = Physics.OverlapSphere(selfPos, range);
            if (colliders.Length != 0)
            {
                foreach (var collider in colliders)
                {
                    var go = collider.gameObject;
                    if (go.layer != ENEMY_LAYER && go.TryGetComponent<IUnitView>(out var unit))
                    {
                        if (unit.IsFighting == false)
                            return unit.SelfTransform.position;
                    }
                }
            }

            return _unit.UnitPriority.CurrentTargetPosition;
        }

        private bool CheckStopDistance(Vector3 position)
        {
            var distance = GetDistanceTo(position);
            return distance < _enemyStopDistance;
        }

        private float GetDistanceTo(Vector3 targetPos)
        {
            var unitPosition = _unit.GetPosition();
            var targetPosition = targetPos;

            return Vector3.Distance(targetPosition, unitPosition);
        }
    }
}
