using System;
using System.Collections.Generic;
using Wave;


namespace Code.MovementOfWorkers.Animations
{
    public class WorkerAnimationController : IWorkerAnimationController
    {
        public event Action ActionMoment;
        public event Action AnimationComplete;
        public bool IsAnimationPlaying { get; private set; }

        private readonly Dictionary<WorkerAnimationType, IWorkerAnimation> 
            _animations;

        private IWorkerAnimation _currentAnimation;


        public WorkerAnimationController(Enemy unit)
        {
            _animations = new Dictionary<WorkerAnimationType, IWorkerAnimation>();
            _animations.Add(WorkerAnimationType.Production, 
                new WorkerProductionAnimation(unit));
        }

        public void PlayAnimation(WorkerAnimationType animationType)
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