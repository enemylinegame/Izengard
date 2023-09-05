using CombatSystem;

namespace EnemyUnit.EnemyStates
{ 
    public class EnemyAttackRangeState : EnemyBaseState
    {
        public EnemyAttackRangeState(
            EnemyModel unit, 
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

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {

        }       
    }
}
