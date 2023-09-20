using Izengard.UnitSystem;
using UnityEngine;

namespace Izengard.EnemySystem
{
    public class EnemyController : IOnController, IOnUpdate, IOnFixedUpdate
    {
        private readonly Vector3 _primaryTarget;
        private readonly IUnit _unit;

        private Vector3 _currentTarget;
        private float _enemyStopDistance;

        private bool _isEnable;
        private bool _isMove;

        public EnemyController(IUnit unit, Vector3 primaryTarget)
        {
            _unit = unit;
            _primaryTarget = primaryTarget;

            _enemyStopDistance = _unit.Model.Offence.OffenceData.MeleeAttackReach;

            _isEnable = false;
            _isMove = false;
        }

        public void Enable()
        {
            _isEnable = true;

            _unit.Enable();

            _currentTarget = _primaryTarget;

            var navigationState = SetupNavigation();
            if(navigationState == true)
            {
                MoveToTarget(_currentTarget);
            }
        }

        private bool SetupNavigation()
        {
            if (_unit.View.UnitNavigation == null)
                return false;

            _unit.View.UnitNavigation.enabled = true;

            if (_unit.View.UnitNavigation.isOnNavMesh)
                _unit.View.UnitNavigation.ResetPath();

            var unitPos = _unit.GetPosition();

            _unit.View.UnitNavigation.Warp(unitPos);

            return true;
        }

        private void MoveToTarget(Vector3 target)
        {
            if (_unit.View.UnitNavigation.isOnNavMesh)
            {
                _unit.View.UnitNavigation.SetDestination(target);
                _isMove = true;
            }
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isEnable == false)
                return;
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            if (_isEnable == false)
                return;

            if (_isMove)
            {
                if (CheckStopDistance(_currentTarget) == true)
                {
                    if (_unit.View.UnitNavigation.isOnNavMesh)
                    {
                        _unit.View.UnitNavigation.ResetPath();
                        _isMove = false;
                    }
                }
            }
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
