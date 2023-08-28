using System;
using Wave;

namespace CombatSystem.UnitEnemy
{
    public class EnemyController : IDisposable, IOnController, IOnUpdate
    {
        private readonly Enemy _enemyUnit;
        private readonly EnemyCore _core;
        private readonly EnemyStatesHolder _statesHolder;
    
        public EnemyController(
            Enemy enemyUnit, 
            Damageable target, 
            IEnemyAnimationController animationController,
            IBulletsController bulletsController)
        {
            _enemyUnit = enemyUnit;

            _core = new EnemyCore(_enemyUnit, target);

            _core.PlanRoute.OnComplete += OnPlaneRouteComplete;

            _statesHolder 
                = new EnemyStatesHolder(_enemyUnit, animationController, _core);
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

        public void OnUpdate(float deltaTime)
        {
            _statesHolder.OnUpdate(deltaTime);
        }
    }
}
