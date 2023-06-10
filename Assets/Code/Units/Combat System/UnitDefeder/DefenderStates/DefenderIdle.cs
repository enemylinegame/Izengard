using System;
using CombatSystem.Interfaces;
using UnityEngine;

namespace CombatSystem.DefenderStates
{
    public class DefenderIdle : DefenderStateBase
    {
        public DefenderIdle(DefenderUnit defenderUnit, Action<DefenderState> setStateDelegate) : 
            base(defenderUnit, setStateDelegate)
        {
            
        }


        public override void OnUpdate()
        {
            
        }

        public override void OnDamaged(IDamageable attacker)
        {
            _setState(DefenderState.Fight);
        }

        public override void AddedTargetInRange(IDamageable target)
        {
            _setState(DefenderState.Fight);
        }

        public override void GoToPosition(Vector3 destination)
        {
            _setState(DefenderState.Going);
        }

        public override void GoToBarrack(Vector3 destination)
        {
            _setState(DefenderState.GotoBarrack);
        }
    }
}