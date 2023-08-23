using Wave;

namespace CombatSystem.UnitEnemy.EnemyStates
{ 
    public class EnemyAttackRangeState : EnemyBaseState
    {
        public EnemyAttackRangeState(
            Enemy unit, 
            IEnemyAnimationController animationController) : base(unit, animationController)
        {

        }

        public override void OnEnter()
        {
            _animationController.PlayAnimation(AnimationType.AttackRange);
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
