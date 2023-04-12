using System;


public interface IEnemyAnimation : IOnFixedUpdate
{
    void PlayAnimation();
    event Action ActionMoment;
    event Action AnimationComplete;
}
