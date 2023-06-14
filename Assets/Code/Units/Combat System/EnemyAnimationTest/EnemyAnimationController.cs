using System;
using System.Collections.Generic;
using Wave;


namespace CombatSystem
{
    public class EnemyAnimationController : IEnemyAnimationController
    {
        public event Action ActionMoment;
        public event Action AnimationComplete;
        public bool IsAnimationPlaying { get; private set; }

        private readonly Dictionary<AnimationType, IEnemyAnimation> _animations;

        private IEnemyAnimation _currentAnimation;


        public EnemyAnimationController(Enemy unit)
        {
            _animations = new Dictionary<AnimationType, IEnemyAnimation>();
            _animations.Add(AnimationType.Attack, new EnemyAttackAnimation(unit));
        }

        public void PlayAnimation(AnimationType animationType)
        {
            _currentAnimation = _animations[animationType];
            _currentAnimation.AnimationComplete += OnAnimationComplete;
            _currentAnimation.ActionMoment += OnActionMoment;
            IsAnimationPlaying = true;
            _currentAnimation.PlayAnimation();
        }

        private void OnAnimationComplete()
        {
            _currentAnimation.AnimationComplete -= OnAnimationComplete;
            IsAnimationPlaying = false;
            AnimationComplete?.Invoke();
        }

        private void OnActionMoment()
        {
            _currentAnimation.ActionMoment -= OnActionMoment;
            ActionMoment?.Invoke();
        }

        public void OnFixedUpdate(float deltaTime)
        {
            if (IsAnimationPlaying) _currentAnimation.OnFixedUpdate(deltaTime);
        }

        public void StopAnimation()
        {
            if (_currentAnimation != null)
            {
                _currentAnimation.AnimationComplete -= OnAnimationComplete;
                _currentAnimation.ActionMoment -= OnActionMoment;
            }
            IsAnimationPlaying = false;
        }
    }
}