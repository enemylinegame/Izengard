using System;


namespace Code.MovementOfWorkers.Animations
{
    public interface IWorkerAnimationController : IOnFixedUpdate
    {
        void PlayAnimation(WorkerAnimationType animationType);
        void StopAnimation();
        event Action ActionMoment;
        event Action AnimationComplete;
        bool IsAnimationPlaying { get; }
    }
}