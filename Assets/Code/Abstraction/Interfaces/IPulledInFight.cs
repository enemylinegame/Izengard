using System;

namespace Abstraction
{
    public interface IFightingObject
    {
        bool IsFighting { get; }

        event Action OnPulledInFight;

        event Action OnReleasedFromFight;
        void PullIntoFight();

        void ReleaseFromFight();
    }
}
