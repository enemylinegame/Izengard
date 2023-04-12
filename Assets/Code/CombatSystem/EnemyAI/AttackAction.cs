using System;
using Wave;


namespace CombatSystem
{
    public class AttackAction : IAction<Damageable>
    {
        public event Action<Damageable> OnComplete;

        private readonly IEnemyAnimationController _animation;
        private readonly Enemy _unit;
        private Damageable _currentTarget;


        public AttackAction(IEnemyAnimationController animation, Enemy unit)
        {
            _animation = animation;
            _unit = unit;
        }

        public void StartAction(Damageable target)
        {
            _currentTarget = target;

            _animation.ActionMoment += OnActionMoment;
            _animation.AnimationComplete += OnAnimationComplete;

            _animation.PlayAnimation(AnimationType.Attack);
        }

        private void OnActionMoment()
        {
            if (_currentTarget != null) _currentTarget.MakeDamage(_unit.Stats.Attack);
            _animation.ActionMoment -= OnActionMoment;
        }

        private void OnAnimationComplete()
        {
            _animation.AnimationComplete -= OnAnimationComplete;
            OnComplete?.Invoke(_currentTarget);
        }
    }
}