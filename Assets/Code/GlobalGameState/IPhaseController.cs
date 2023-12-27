using System;

namespace GlobalGameState
{
    public interface IPhaseController : IOnController
    {
        event Action OnPhaseEnd;

        public void StartPhase();

        public void EndPhase();
    }
}
