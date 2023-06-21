using System;

using CombatSystem.Interfaces;
using UnityEngine;


namespace CombatSystem.DefenderStates
{
    public abstract class DefenderStateBase
    {
        
        protected DefenderUnit _defenderUnit;
        protected Action<DefenderState> _setState;

        protected DefenderStateBase(DefenderUnit defenderUnit, Action<DefenderState> setStateDelegate)
        {
            _defenderUnit = defenderUnit;
            _setState = setStateDelegate;
        }


        // public virtual void Dispose()
        // {
        //     
        // }

        public virtual void StartState() { }
        
        public virtual void OnUpdate() { }
        
        public virtual void OnDamaged(IDamageable attacker) { }

        public virtual void AddedTargetInRange() { }

        public virtual void GoToPosition(Vector3 destination) { }

        public virtual void GoToBarrack(Vector3 destination) { }
        
        public virtual void ExitFromBarrack() { }
    }
}