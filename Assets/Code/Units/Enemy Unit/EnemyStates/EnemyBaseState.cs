using CombatSystem;

namespace EnemyUnit.EnemyStates
{
    public abstract class EnemyBaseState
    {
        protected readonly EnemyModel _unit;
        protected readonly IEnemyAnimationController _animationController;
        
        public EnemyBaseState(
            EnemyModel unit, 
            IEnemyAnimationController animationController) 
        {
            _unit = unit;
            _animationController = animationController;
        }

        public abstract void OnEnter();

        public abstract void OnExit();

        public abstract void OnUpdate();
        public abstract void OnFixedUpdate();
    }
}
