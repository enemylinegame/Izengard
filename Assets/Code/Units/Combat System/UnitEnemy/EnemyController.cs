using System;
using UnityEngine.AI;
using Wave;

namespace CombatSystem.UnitEnemy
{
    public class EnemyController : IDisposable
    {
        private readonly Enemy _enemyUnit;
        private readonly EnemyStatesHolder _statesHolder;

        private readonly PlanRouteAction _planRoute;

        private Damageable _currentTarget;
        
        public EnemyController(
            Enemy enemyUnit, 
            Damageable target, 
            IEnemyAnimationController animationController,
            IBulletsController bulletsController)
        {
            _enemyUnit = enemyUnit;
            
            _currentTarget = target;

            var navMesh 
                = _enemyUnit.RootGameObject.GetComponent<NavMeshAgent>();
            
            _planRoute = new PlanRouteAction(navMesh);
            _planRoute.OnComplete += OnPlaneRouteComplete;


            _statesHolder 
                = new EnemyStatesHolder(_enemyUnit, animationController, _planRoute, _currentTarget);
        }

        private void OnPlaneRouteComplete(Damageable target)
        {
            if (target == null || target.IsDead)
            {
                _statesHolder.ChangeState(EnemyStateType.SearchForTarget);
            }
            else
            {
                _statesHolder.ChangeState(EnemyStateType.Idle);
            }
        }

        public void Dispose()
        {
            _statesHolder?.Dispose();
        }
    }
}
