using GlobalGameState;
using System;

namespace Code.GlobalGameState
{
    public class PeacePhaseConttoller : IPhaseController
    {
        public event Action OnPhaseEnd;

        public void StartPhase()
        {
            
        }

        public void EndPhase()
        {
            OnPhaseEnd?.Invoke();
        }        
    }
}