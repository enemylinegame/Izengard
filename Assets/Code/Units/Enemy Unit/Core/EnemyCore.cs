namespace EnemyUnit.Core
{
    public class EnemyCore
    {
        private readonly Damageable _primaryTarget;
        private readonly IAction<Damageable> _findTarget;
        private readonly IAction<Damageable> _planRoute;
        private readonly IAction<Damageable> _checkAttackDistance;

        private Damageable _currentTarget;

        public Damageable CurrentTarget => _currentTarget;
        public IAction<Damageable> FindTarget => _findTarget;

        public IAction<Damageable> PlanRoute => _planRoute;

        public IAction<Damageable> CheckAttackDistance => _checkAttackDistance;
     

        public EnemyCore(EnemyModel model, EnemyView view, Damageable primaryTarget)
        {
            _primaryTarget = primaryTarget;
            _currentTarget = _primaryTarget;

            _findTarget = new FindTargetAction(model, view, _primaryTarget);

            var navmesh = view.NavMesh;
            _planRoute = new PlanRouteAction(view);
            _checkAttackDistance = new CheckAttackDistance(model, view, model.Stats.AttackRange);
        }

        public void ChangeTarget(Damageable newTarget)
        {
            _currentTarget = newTarget;
        }


    }
}
