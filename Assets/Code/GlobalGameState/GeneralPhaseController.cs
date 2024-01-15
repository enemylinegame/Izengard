using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GlobalGameState
{
    public class GeneralPhaseController : IPhaseController
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
