using CombatSystem;
using EnemyUnit.Core;
using EnemyUnit.Interfaces;

namespace EnemyUnit
{
    public class EnemyController : IEnemyController
    {
        private readonly EnemyModel _model;
        private readonly EnemyView _view;
        private readonly EnemyCore _core;
        private readonly EnemyStatesHolder _statesHolder;
    
        public EnemyController(
            EnemyModel model, 
            EnemyView view,
            EnemyCore core,
            EnemyStatesHolder statesHolder)
        {
            _model = model;
            _view = view;

            _core = core;

            _core.PlanRoute.OnComplete += OnPlaneRouteComplete;

            _statesHolder = statesHolder;
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

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            
        }
    }
}
