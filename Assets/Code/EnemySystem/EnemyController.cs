using Izengard.UnitSystem;
using Izengard.UnitSystem.Enum;
using UnityEngine;
namespace Izengard.EnemySystem
{
    public class EnemyController : IOnController, IOnUpdate, IOnFixedUpdate
    {
        private const int ENEMY_LAYER = 8;

        private readonly Vector3 _primaryTarget;
        private readonly IUnit _unit;

        private Vector3 _currentTarget;
        private float _enemyStopDistance;

        private bool _isEnable;
        private bool _isMove;
        private bool _isReachTarget;

        public EnemyController(IUnit unit, Vector3 primaryTarget)
        {
            _unit = unit;
            _primaryTarget = primaryTarget;

            if (_unit.Model.Type == UnitType.Melee)
            {
                _enemyStopDistance = _unit.Model.Offence.OffenceData.MeleeAttackReach;
            }
            else if (_unit.Model.Type == UnitType.Range) 
            {
                _enemyStopDistance = _unit.Model.Offence.OffenceData.RangedAttackMaxRange;
            }

            _unit.View.OnPulledInFight += OnPullInFight;

            _isEnable = false;
            _isMove = false;
            _isReachTarget = false;
        }

        private void OnPullInFight()
        {
            Debug.Log($"Enemy[{_unit.Model.Type}] - {_unit.Id} is in fight!");

            _isEnable = false;
        }

        public void Enable()
        {
            _isEnable = true;

            _unit.Enable();

            _currentTarget = _primaryTarget;

            _unit.Navigation.Enable();
            MoveToTarget(_currentTarget);
        }

        private void MoveToTarget(Vector3 target)
        {
            _unit.Navigation.Stop();

            _unit.Navigation.MoveTo(target);
            _isMove = true;
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
                    _currentTarget = _primaryTarget;

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

            return _primaryTarget;
        }

        private bool CheckStopDistance(Vector3 position)
        {
            var distance = GetDistanceTo(position);
            return distance < _enemyStopDistance;
        }

        public float GetDistanceTo(Vector3 targetPos)
        {
            var unitPosition = _unit.GetPosition();
            var targetPosition = targetPos;

            return Vector3.Distance(targetPosition, unitPosition);
        }
    }
}
