using System;
using UnityEngine;

namespace CombatSystem.DefenderStates
{
    public class DefenderDying : DefenderStateBase
    {

        private readonly Collider _collider;
            
        public DefenderDying(DefenderUnit defenderUnit, Action<DefenderState> setStateDelegate, GameObject rootGO) : 
            base(defenderUnit, setStateDelegate)
        {
            _collider = rootGO.GetComponent<Collider>();
        }
        
        public override void StartState()
        {
            _collider.enabled = false;
        }
        
    }
}