using Wave;

namespace CombatSystem.UnitEnemy.EnemyStates
{
    public class EnemyAttackState : EnemyBaseState
    {
        public EnemyAttackState(
            Enemy unit, 
            IEnemyAnimationController animationController) : base(unit, animationController)
        {
        }

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }
    }
}
