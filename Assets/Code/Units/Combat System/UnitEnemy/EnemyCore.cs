using UnityEngine.AI;
using Wave;

namespace CombatSystem.UnitEnemy
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
     

        public EnemyCore(Enemy unit, Damageable primaryTarget)
        {
            _primaryTarget = primaryTarget;
            _currentTarget = _primaryTarget;

            _findTarget = new FindTargetAction(unit, _primaryTarget);

            var navmesh = unit.RootGameObject.GetComponent<NavMeshAgent>();
            _planRoute = new PlanRouteAction(navmesh);
            _checkAttackDistance = new CheckAttackDistance(unit, navmesh, unit.Stats.AttackRange);
        }

        public void ChangeTarget(Damageable newTarget)
        {
            _currentTarget = newTarget;
        }


    }
}
