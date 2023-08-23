using Wave;

namespace CombatSystem.UnitEnemy.EnemyStates
{
    public class EnemyIdleState : EnemyBaseState
    {
        public EnemyIdleState(
            Enemy unit, 
            IEnemyAnimationController animationController) : base(unit, animationController)
        {

        }

        public override void OnEnter()
        {
            _animationController.PlayAnimation(AnimationType.Idle);
        }

        public override void OnExit()
        {
            _animationController.StopAnimation();
        }

        public override void OnUpdate()
        {

        }

        public override void OnFixedUpdate()
        {

        }
    }
}
