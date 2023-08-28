using Wave;

namespace CombatSystem.UnitEnemy.EnemyStates
{
    public class EnemyMoveState : EnemyBaseState
    {
        private readonly PlanRouteAction _planRoute;
        private readonly Damageable _currentTarget;

        public EnemyMoveState(
            Enemy unit, 
            IEnemyAnimationController animationController,
            EnemyCore core) : base(unit, animationController)
        {
            _planRoute = (PlanRouteAction)core.PlanRoute;
            _currentTarget = core.CurrentTarget;
        }

        public override void OnEnter()
        {
            _animationController.PlayAnimation(AnimationType.Move);
            _planRoute.StartAction(_currentTarget);
        }

        public override void OnExit()
        {
            _animationController.StopAnimation();
            _planRoute.ClearTarget();
        }

        public override void OnUpdate()
        {

        }

        public override void OnFixedUpdate()
        {

        }


    }
}
