using Interfaces;
using System;
using System.Collections.Generic;


namespace Wave.Interfaces
{
    public interface IPhaseWaiting
    {
        event Action PhaseEnded;
        void StartPhase();
    }
}