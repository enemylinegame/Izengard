using System;

namespace Code.MovementOfWorkers.Animations
{
    public interface IWorkerAnimation : IOnFixedUpdate
    {
        void PlayAnimation();
        event Action ActionMoment;
        event Action AnimationComplete;
    }
}
