using Wave;

namespace CombatSystem.UnitEnemy.EnemyStates
{
    public class EnemyDyingState : EnemyBaseState
    {
        public EnemyDyingState(
            Enemy unit, 
            IEnemyAnimationController animationController) : base(unit, animationController)
        {

        }

        public override void OnEnter()
        {
            _animationController.PlayAnimation(AnimationType.Dying);
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
