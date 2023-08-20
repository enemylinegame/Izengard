using Wave;

namespace CombatSystem.UnitEnemy.EnemyStates
{
    public abstract class EnemyBaseState
    {
        protected readonly Enemy _unit;
        protected readonly IEnemyAnimationController _animationController;
        
        public EnemyBaseState(
            Enemy unit, 
            IEnemyAnimationController animationController) 
        {
            _unit = unit;
            _animationController = animationController;
        }

        public abstract void OnEnter();

        public abstract void OnExit();
    }
}
