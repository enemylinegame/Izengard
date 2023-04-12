using System;


namespace Wave.Interfaces
{
    public interface IPhaseWaiting
    {
        event Action PhaseEnded;
        void StartPhase();
    }
}