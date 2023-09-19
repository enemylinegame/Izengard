using Izengard.UnitSystem;
using UnityEngine;

namespace Izengard.EnemySystem
{
    public class EnemyController : IOnController, IOnUpdate, IOnFixedUpdate
    {
        private readonly Vector3 _primaryTarget;
        private readonly IUnit _unit;

        private bool _isEnable;

        public EnemyController(IUnit unit, Vector3 primaryTarget)
        {
            _unit = unit;
            _primaryTarget = primaryTarget;

            _isEnable = false;
        }

        public void Enable()
        {
            _isEnable = true;

            _unit.Enable();

            var navigationState = SetupNavigation();
            if(navigationState == true)
            {
                MoveToTarget(_primaryTarget);
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
        }
    }
}
